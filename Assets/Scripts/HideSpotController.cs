using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideSpotController : MonoBehaviour
{
    public GameObject floatingText;
    private GameObject newText;

    public bool hasSuperSecretBag = false;

    private string playerName = "None";
    private bool in_zone = false;
    private bool active = true;

    public void Search()
    {
        if(in_zone)
        {
            if (GameManager.gameMode == 3)
            {
                // Search for Super Secret Bag of Candies
                StartCoroutine(WaitForSearching());
                if (hasSuperSecretBag)
                {
                    GameObject.Find(playerName + "UI").GetComponent<Rigidbody2D>().
                        AddForce(Vector3.up * 8.5f, ForceMode2D.Impulse);
                    GameManager.HideSuperSecretBagOfCandies();

                    if (newText == null)
                        CreateText();
                    newText.GetComponent<TextMesh>().text = "You found the " +
                        "\nSuper Secret Bag of Candies!!!";
                    newText.GetComponent<FadeOut>().FadeInText();

                    GameObject.FindWithTag(playerName).GetComponent<PlayerMovement>().
                        Candies(gameObject.transform.position, 1);
                }
                else
                {
                    if (newText == null)
                        CreateText();
                    newText.GetComponent<TextMesh>().text = "Nothing found...";
                    newText.GetComponent<FadeOut>().FadeInText();
                    StartCoroutine(WaitToFadeOut());
                }
            }
            else
            {
                // Drop candies
                int number = Random.Range(1, 10);
                if (number >= 4)
                {
                    GameObject.Find(playerName + "UI").GetComponent<Rigidbody2D>().
                        AddForce(Vector3.up * 8.5f, ForceMode2D.Impulse);
                    
                    GameObject.FindWithTag(playerName).GetComponent<PlayerMovement>().
                        Candies(gameObject.transform.position);
                    int candies = GameObject.FindWithTag(playerName).
                        GetComponent<PlayerMovement>().numCandies;
                    newText.GetComponent<TextMesh>().text =
                        "Found " + candies + " candies!!";
                    newText.GetComponent<FadeOut>().FadeInText();

                    active = false;
                    StartCoroutine(WaitToFadeOut());
                }
                else
                {
                    newText.GetComponent<TextMesh>().text = "Nothing found...";
                    newText.GetComponent<FadeOut>().FadeInText();
                    StartCoroutine(WaitToFadeOut());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kid") || other.CompareTag("Spooky"))
        {
            if(active)
            {
                CreateText();

                newText.GetComponent<TextMesh>().text = "Press " + 
                    other.gameObject.GetComponent<PlayerMovement>().InteractKey.ToString() +
                    " to search...";
                newText.GetComponent<FadeOut>().FadeInText();
                playerName = other.gameObject.tag;
                in_zone = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Kid") || other.CompareTag("Spooky"))
        {
            in_zone = false;
            if (newText != null)
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
                               gameObject.transform.position.x,
                               gameObject.transform.position.y + 0.75f,
                               gameObject.transform.position.z);
        newText = Instantiate(floatingText, position, Quaternion.identity);
        Renderer textRenderer = newText.GetComponent<Renderer>();
        textRenderer.sortingOrder = 6;
    }

    private IEnumerator WaitToFadeOut()
    {
        yield return new WaitForSeconds(1);
        newText.GetComponent<FadeOut>().FadeOutText();
        if(!active)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            SpriteRenderer image = gameObject.GetComponent<SpriteRenderer>();
            image.color = new Color(image.color.r,
                                    image.color.g,
                                    image.color.b, 0f);
            yield return new WaitForSeconds(12);
            active = true;
            gameObject.GetComponent<Collider2D>().enabled = true;
            image.color = new Color(image.color.r,
                                    image.color.g,
                                    image.color.b, 1f);
        }
    }

    private IEnumerator WaitForSearching()
    {
        newText.GetComponent<TextMesh>().text = "Searching...";
        newText.GetComponent<FadeOut>().FadeInText();
        yield return new WaitForSeconds(1);
    }
}
