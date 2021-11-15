using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Spooky;
    public GameObject Kid;

    public GameObject SpookyLocal;
    public GameObject KidLocal;

    private GameObject SpookyInstance;
    private GameObject KidInstance;

    public Transform spawnSpooky;
    public Transform spawnKid;

    public GameObject floatingText;
    public Transform BeginTextPosition;
    private GameObject newText;

    new public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.room == 2)
        {
            if (GameManager.player == 1 && Spooky != null)
            {
                SpookyInstance = PhotonNetwork.Instantiate(Spooky.name,
                                 spawnSpooky.position,
                                 Quaternion.identity);
            }
            else if (GameManager.player == 2 && Kid != null)
            {
                KidInstance = PhotonNetwork.Instantiate(Kid.name,
                                 spawnKid.position,
                                 Quaternion.identity);
            }
            StartCoroutine(Go());
        }
        else if (GameManager.room == 3)
        {
            print("Local 1v1");
            if (Spooky != null)
            {
                //Instantiate(Spooky, spawnSpooky.position, Quaternion.identity);
                SpookyLocal.SetActive(true);
                SpookyInstance = GameObject.FindWithTag("Spooky");
                print(SpookyInstance.name);
            }
                
            if (Kid != null)
            {
                //Instantiate(Kid, spawnKid.position, Quaternion.identity);
                KidLocal.SetActive(true);
                KidInstance = GameObject.FindWithTag("Kid");
                print(KidInstance.name);
            }
            StartCoroutine(Go());
        }
        else if (GameManager.room == 4)
        {
            if(camera.gameObject.name == "Camera1")
                camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            if (Spooky != null)
            {
                SpookyLocal.SetActive(true);
                SpookyInstance = GameObject.FindWithTag("Spooky");
                print(SpookyInstance.name);
            }
            StartCoroutine(ShowGo());
        }
    }

    private IEnumerator Go()
    {
        yield return new WaitUntil(() =>
        GameObject.FindWithTag("Kid") != null && GameObject.FindWithTag("Spooky") != null);
        yield return new WaitForSeconds(0.1f);

        GameObject.FindWithTag("Spooky").GetComponent<PlayerMovement>().letControls = false;
        GameObject.FindWithTag("Kid").GetComponent<PlayerMovement>().letControls = false;

        StartCoroutine(ShowGo());
        yield return new WaitForSeconds(3.0f);

        GameObject.FindWithTag("Spooky").GetComponent<PlayerMovement>().letControls = true;
        GameObject.FindWithTag("Kid").GetComponent<PlayerMovement>().letControls = true;

        yield return null;
    }

    private IEnumerator ShowGo()
    {
        if (Spooky != null)
        {
            for (int i = 3; i > 0; i--)
            {
                newText = Instantiate(floatingText, BeginTextPosition.position,
                                       Quaternion.identity, BeginTextPosition);
                Renderer textRenderer = newText.GetComponent<Renderer>();
                textRenderer.sortingOrder = 6;
                newText.layer = 8;
                newText.GetComponent<TextMesh>().characterSize = 0.9f;
                newText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                newText.GetComponent<FadeOut>().lifeTime = 1.0f;

                newText.GetComponent<TextMesh>().text =
                    GameManager.gameModeNames[GameManager.gameMode] + "\n" + i + "...";
                newText.GetComponent<FadeOut>().FadeInText();
                yield return new WaitForSeconds(1.0f);
            }
            newText = Instantiate(floatingText, BeginTextPosition.position,
                                       Quaternion.identity, BeginTextPosition);
            Renderer textRenderer2 = newText.GetComponent<Renderer>();
            textRenderer2.sortingOrder = 6;
            newText.layer = 8;
            newText.GetComponent<TextMesh>().characterSize = 0.9f;
            newText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
            newText.GetComponent<FadeOut>().lifeTime = 1.0f;

            newText.GetComponent<TextMesh>().text = "Go!";
            newText.GetComponent<FadeOut>().FadeInText();
        }
        else
        {
            yield return new WaitForSeconds(3.0f);
        }
    }
}
