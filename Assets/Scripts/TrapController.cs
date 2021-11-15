using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public string owner = "None";
    public GameObject floatingText;
    public bool in_zone = false;
    private string trap;
    private string player;
    private string showText;
    private GameObject gameObjectParent;
    private GameObject newText;

    void Start()
    {
        gameObjectParent = gameObject.transform.parent.gameObject;
        trap = gameObjectParent.name;
    }

    public void AccessControl()
    {
        if(in_zone)
        {
            if (newText == null)
                CreateText();
            owner = player;
            newText.GetComponent<TextMesh>().text = "Control granted!";
            newText.GetComponent<FadeOut>().FadeInText();
        }
    }

    public void Trigger()
    {
        if(in_zone)
        {
            if (newText == null)
                CreateText();
            owner = player;
            gameObjectParent.GetComponent<Animator>().SetTrigger("Activate");
            newText.GetComponent<TextMesh>().text = showText + " triggered!";
            newText.GetComponent<FadeOut>().FadeInText();
            //print(trap + " on!");
        }

    }

    public void ShowOwner()
    {
        if (in_zone)
        {
            if (newText == null)
                CreateText();
            newText.GetComponent<TextMesh>().text = "Current owner: " + owner;
            newText.GetComponent<FadeOut>().FadeInText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kid") || other.CompareTag("Spooky"))
        {
            in_zone = true;
            player = other.gameObject.tag;
            //print(trap + ": Z to access control");
            //print(trap + ": X to trigger trap");
            //print(trap + ": C to view owner");

            CreateText();
            newText.GetComponent<FadeOut>().FadeInText();

            switch (trap)
            {
                case "Arrows":
                    newText.GetComponent<TextMesh>().text = "Arrows trap";
                    break;
                case "Peaks":
                    newText.GetComponent<TextMesh>().text = "Peaks trap";
                    break;
                default:
                    newText.GetComponent<TextMesh>().text = "Flame thrower";
                    break;
            }

            showText = newText.GetComponent<TextMesh>().text;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Kid") || other.CompareTag("Spooky"))
        {
            in_zone = false;
            if(newText != null)
            {
                newText.GetComponent<FadeOut>().forceOut = true;
                newText.GetComponent<FadeOut>().FadeOutText();
            }
        }
    }

    private void CreateText()
    {
        //Instantiate Text
        Vector3 position = new Vector3(
                               gameObjectParent.transform.position.x,
                               gameObjectParent.transform.position.y + 0.75f,
                               gameObjectParent.transform.position.z);
        newText = Instantiate(floatingText, position, Quaternion.identity);
        Renderer textRenderer = newText.GetComponent<Renderer>();
        textRenderer.sortingOrder = 6;
    }
}
