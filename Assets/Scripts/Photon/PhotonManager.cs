using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public void ExitGame()
    {
        if (GameManager.room == 2)
        {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            GameManager.room = 5;
            SceneManager.LoadScene("Lobby");
        }
    }

    public override void OnLeftRoom()
    {
        GameManager.room = 1;
        PhotonNetwork.LoadLevel("Lobby");
    }
}
