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

    void Update()
    {
        if (Input.GetKeyDown("z") && in_zone)
        {
            if(GameManager.gameMode == 3)
            {
                // Search for Super Secret Bag of Candies
                StartCoroutine(WaitForSearching());
                if (hasSuperSecretBag)
                {
                    GameManager.whoFoundSuperSecretBag = playerName;
                    newText.GetComponent<TextMesh>().text = "You found the " + 
                        "\nSuper Secret Bag of Candies!!!";
                    newText.GetComponent<FadeOut>().FadeInText();
                }
                else
                {
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
                    int candies = Random.Range(1, 6);
                    newText.GetComponent<TextMesh>().text =
                        "Found " + candies + " candies!!";
                    newText.GetComponent<FadeOut>().FadeInText();
                    StartCoroutine(ShowCandies((candies > 3 ? 3 : candies)));

                    if (playerName == "Spooky")
                    {
                        GameManager.spookyCandies += candies;
                        print(GameManager.spookyCandies);

                        GameObject.Find(playerName + "Score").
                            GetComponent<Text>().text = GameManager.spookyCandies.ToString();
                    }
                    else
                    {
                        GameManager.kidCandies += candies;
                        print(GameManager.kidCandies);

                        GameObject.Find(playerName + "Score").
                            GetComponent<Text>().text = GameManager.kidCandies.ToString();
                    }


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
        if (other.CompareTag("Player"))
        {
            if(active)
            {
                //Instantiate Text
                Vector3 position = new Vector3(
                                           gameObject.transform.position.x,
                                           gameObject.transform.position.y + 0.75f,
                                           gameObject.transform.position.z);
                newText = Instantiate(floatingText, position, Quaternion.identity);
                Renderer textRenderer = newText.GetComponent<Renderer>();
                textRenderer.sortingOrder = 6;

                newText.GetComponent<TextMesh>().text = "Press Z to search...";
                newText.GetComponent<FadeOut>().FadeInText();
                playerName = other.gameObject.name;
                in_zone = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            in_zone = false;
            if (newText != null)
            {
                newText.GetComponent<FadeOut>().forceOut = true;
                newText.GetComponent<FadeOut>().FadeOutText();
            }
        }
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
            yield return new WaitForSeconds(8);
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

    private IEnumerator ShowCandies(int candies)
    {
        for (int i = 0; i < candies; i++)
        {
            if (GameManager.candies[i] != null)
            {
                Vector3 position = new Vector3(
                                   gameObject.transform.position.x,
                                   gameObject.transform.position.y + 0.75f,
                                   gameObject.transform.position.z);
                GameObject candyObject = Instantiate(GameManager.candies[i],
                    position, Quaternion.identity);
                Renderer textRenderer = candyObject.GetComponent<Renderer>();
                textRenderer.sortingOrder = 6;
                yield return new WaitForSeconds(0.12f);
            }
        }
    }
}
