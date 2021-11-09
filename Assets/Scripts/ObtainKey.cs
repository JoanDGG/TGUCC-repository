using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject MainDoor = GameObject.Find("MainDoor");
            MainDoor.GetComponent<DoorAnimation>().Open();
        }
    }
}
