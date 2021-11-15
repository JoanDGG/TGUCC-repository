using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapEffect : MonoBehaviour
{
    public GameObject floatingText;
    private GameObject newText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.NPCNames.Contains(other.gameObject))
        {
            //Instantiate Text
            Vector3 position = new Vector3(
                                       gameObject.transform.position.x,
                                       gameObject.transform.position.y + 1.2f,
                                       gameObject.transform.position.z);
            newText = Instantiate(floatingText, position, Quaternion.identity);
            Renderer textRenderer = newText.GetComponent<Renderer>();
            textRenderer.sortingOrder = 6;

            other.gameObject.GetComponent<NpcAI>().GetScared();
            string name = gameObject.transform.GetChild(0).
                GetComponent<TrapController>().owner;

            int candies = Random.Range(1, 3);
            newText.GetComponent<TextMesh>().text = name + " got " + candies + " candies!!";
            newText.GetComponent<FadeOut>().FadeInText();

            GameObject.Find(name + "UI").GetComponent<Rigidbody2D>().
                AddForce(Vector3.up * 8.5f, ForceMode2D.Impulse);

            print("Give candies to " + GameObject.FindWithTag(name).name);

            GameObject.FindWithTag(name).GetComponent<PlayerMovement>().
                Candies(gameObject.transform.position, candies);
        }
    }
}
