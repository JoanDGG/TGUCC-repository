using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public string owner = "None";
    private bool active = false;
    private bool in_zone = false;
    private string trap;
    private string player;

    void Start()
    {
        trap = gameObject.transform.parent.name;
    }

    void Update()
    {
        if (in_zone)
        {
            if (Input.GetKeyDown("x"))
            {
                print("Control granted");
                owner = player;
                print("Current owner: " + owner);
            }
            else if (Input.GetKeyDown("z"))
            {
                owner = player;
                print(trap + " triggered!");
                Trigger();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            in_zone = true;
            player = other.gameObject.name;
            print(trap + ": X to access control");
            print(trap + ": Z to trigger trap");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            in_zone = false;
        }
    }

    public void Trigger()
    {
        active = !active;
        gameObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("Active", active);
        print(trap + (active ? " on" : " off"));
    }
}
