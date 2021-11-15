using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    public GameObject roomListing;
    public GameObject content;

    private List<GameObject> listings = new List<GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = listings.FindIndex(x => x.GetComponent<RoomListing>().RoomInfo.Name
                                               == info.Name);
                if(index != -1)
                {
                    Destroy(listings[index]);
                    listings.RemoveAt(index);
                }
            }
            else
            {
                GameObject listing = Instantiate(roomListing, content.transform);
                if (listing.GetComponent<RoomListing>() != null)
                {
                    listing.GetComponent<RoomListing>().SetRoomInfo(info);
                    listings.Add(listing);
                }
            }
        }
    }
}
