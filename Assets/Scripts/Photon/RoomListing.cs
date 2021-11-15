using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    public Text textContent;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        textContent.text = roomInfo.Name + ": " + roomInfo.MaxPlayers;
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        GameObject.Find("CreateAndJoinRooms").GetComponent<CreateAndJoinRooms>().joining = true;
    }
}
