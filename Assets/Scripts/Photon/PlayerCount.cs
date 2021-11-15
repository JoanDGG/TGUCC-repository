using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCount : MonoBehaviourPunCallbacks
{
    public int countPlayersOnline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public override void OnLobbyStatisticsUpdate()
    //{
    //    countPlayersOnline = PhotonNetwork.countOfPlayers;
    //    print(countPlayersOnline.ToString() + " Players Online");
    //}
}
