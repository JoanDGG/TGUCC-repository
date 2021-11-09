using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.NPCNames.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<NpcAI>().GetScared();
            string name = gameObject.transform.GetChild(0).
                GetComponent<TrapController>().owner;
            int candies = Random.Range(1, 3);
            print(name + " got " + candies + " candies!!");
            StartCoroutine(ShowCandies(candies));

            if (name == "Spooky")
            {
                GameManager.spookyCandies += candies;
                print(GameManager.spookyCandies);

                GameObject.Find(name + "Score").
                    GetComponent<Text>().text = GameManager.spookyCandies.ToString();
            }
            else
            {
                GameManager.kidCandies += candies;
                print(GameManager.kidCandies);

                GameObject.Find(name + "Score").
                    GetComponent<Text>().text = GameManager.kidCandies.ToString();
            }
        }
    }

    private IEnumerator ShowCandies(int candies)
    {
        for (int i = 0; i < candies; i++)
        {
            if(GameManager.candies[i] != null)
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
