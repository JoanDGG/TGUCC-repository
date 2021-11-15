using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public GameObject canvasOnline;
    public GameObject canvasLocal;

    public InputField parameterInput;
    public InputField createInput;
    public InputField joinInput;

    public Text Warning;
    public Text NickName;
    public Text count;

    public GameObject Door;
    public GameObject Kid;

    private RoomOptions roomOptions = new RoomOptions();
    private PhotonView view;

    public bool joining = false;

    private ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();


    // Start is called before the first frame update
    void Start()
    {
        roomOptions.MaxPlayers = 2;
        NickName.text = PhotonNetwork.NickName;
        if(GameManager.room == 1)
        {
            canvasLocal.SetActive(false);
            Kid.SetActive(false);
        }
        else
        {
            canvasOnline.SetActive(false);
            parameterInput = GameObject.Find("InputParameter2").GetComponent<InputField>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        if (createInput.text != "")
        {
            ChangeValues();
            myCustomProperties["GameMode"] = GameManager.gameMode;
            PhotonNetwork.LocalPlayer.CustomProperties = myCustomProperties;
            PhotonNetwork.JoinOrCreateRoom(createInput.text, roomOptions, TypedLobby.Default);
            GameManager.player = 1;
        }
        else
        {
            Warning.text = "No empty room names pls";
        }
    }

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Warning.text = "No match found, try creating a room";
        CreateRoom();
    }

    public void ChangeValues()
    {
        float value;
        float.TryParse(parameterInput.text, out value);
        value = Mathf.Abs(value);
        switch (GameManager.gameMode)
        {
            case 2:
                GameManager.totalMinutes = Mathf.Clamp(value, 1.0f, 10.0f);
                parameterInput.text = GameManager.totalMinutes + "";
                break;
            case 3:
                GameManager.SuperSecretBagRounds = Mathf.Clamp((int)value, 1, 10);
                parameterInput.text = GameManager.SuperSecretBagRounds + "";
                break;
            default:
                GameManager.totalCandies = Mathf.Clamp((int)value, 5, 99);
                parameterInput.text = GameManager.totalCandies + "";
                break;
        }
    }

    public void JoinRoom()
    {
        if (joinInput.text != "")
        {
            PhotonNetwork.JoinRoom(joinInput.text);
            joining = true;
        }
        else
        {
            Warning.text = "Please insert a room name";
        }
    }

    public override void OnJoinedRoom()
    {
        GameManager.room = 2;
        PhotonNetwork.CurrentRoom.SetCustomProperties(myCustomProperties);
        Warning.text = "Room created, waiting for people to join...";
        if (joining)
        {
            GameManager.gameMode = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"];
            print("NEW Gamemode " + GameManager.gameMode);
            GameManager.player = 2;
            print("PLAYER " + GameManager.player);
            Warning.text = "Match found! Waiting for host to start the game...";
            Door.GetComponent<Animator>().SetBool("Open", true);
            Destroy(Door.GetComponent<Collider2D>());
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("player " + newPlayer.NickName + "join");
        Warning.text = "Match found! Proceed to enter the game";
        Door.GetComponent<Animator>().SetBool("Open", true);
        Destroy(Door.GetComponent<Collider2D>());
    }

    public void Play()
    {
        GameManager.room = 3;
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        if(GameManager.room == 2)
        {
            StartCoroutine(DisconnectAndLoad());
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }
}
