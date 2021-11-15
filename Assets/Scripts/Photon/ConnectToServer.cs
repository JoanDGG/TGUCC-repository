using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject LoadingText;
    public InputField username;

    // Start is called before the first frame update
    void Start()
    {
        LoadingText.SetActive(false);
    }

    public void Connect()
    {
        if(username.text != "")
        {
            PhotonNetwork.NickName = username.text;
        }
        else
        {
            SetRandomName();
        }
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
        LoadingText.SetActive(true);
    }

    public void SetRandomName()
    {
        PhotonNetwork.NickName = AINamesGenerator.Utils.GetRandomName();
        username.text = PhotonNetwork.NickName;
    }

    public override void OnConnectedToMaster()
    {
        //SceneManager.LoadScene("Lobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LoadingText.SetActive(false);
        GameManager.room = 1;
        SceneManager.LoadScene("Lobby");
    }
}
