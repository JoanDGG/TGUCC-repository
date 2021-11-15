using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int player = 1; // Either player 1 or 2
    public static int room = 0;

    /*
     * Rooms:
     * 0.- Main Menu
     * 1.- Lobby
     * 2.- GameScene Online
     * 3.- GameScene Local 1v1
     * 4.- GameScene Local Training
     * 5.- Lobby Local
     * 
     */

    public static int gameMode = 0;
    /* 
     * Game Modes:
     * 0.- First to get 33 candies from scares!         -> "Trick or treat!"
     * 1.- First to get 33 candies from places!         -> "Candy hunt"
     * 2.- Most candies in 6 minutes!                   -> "Rushdown"
     * 3.- Find the super secret bag of candies first!  -> "Super Secret Bag of Candies! (SSBC)"
     * 4.- Training
     * 
     */

    public GameObject Traps;
    public GameObject hideSpots;

    public GameObject candies1;
    public GameObject candies2;
    public GameObject candies3;
    public static GameObject[] candies;

    public GameObject logo;
    
    public static List<GameObject> NPCNames = new List<GameObject>();
    public static HideSpotController[] hideSpotsList;
    public static int spookyCandies = 0;
    public static int kidCandies = 0;
    
    public static int totalCandies = 33; //Min: 5, Max: 99
    public static float totalMinutes = 6f; //Min: 1, Max: 10
    public static int SuperSecretBagRounds = 3; //Min: 1, Max: 10

    public static string[] gameModeNames = {"Trick or Treat!",
                                                "Candy Hunt!",
                                                  "Rushdown!",
                               "Super Secret Bag of Candies!",
                                                 "Training" };

    public GameObject floatingText;
    public Transform BeginTextPosition;
    private GameObject newText;

    public static List<KeyCode> ControlsP1 = new List<KeyCode> { KeyCode.Z, KeyCode.X, KeyCode.C };
    public static List<KeyCode> ControlsP2 = new List<KeyCode> { KeyCode.B, KeyCode.N, KeyCode.M };

    public static List<KeyCode> MovementP1 = new List<KeyCode> { KeyCode.A, KeyCode.D,
                                                                 KeyCode.W, KeyCode.S };
    public static List<KeyCode> MovementP2 = new List<KeyCode> { KeyCode.LeftArrow, KeyCode.RightArrow,
                                                                 KeyCode.UpArrow, KeyCode.DownArrow };


    void Start()
    {
        GameObject gameObjectParent = GameObject.Find("NPCs");
        foreach (Transform t in gameObjectParent.transform)
        {
            NPCNames.Add(t.gameObject);
        }
        candies = new GameObject[] { candies1, candies2, candies3 };
        hideSpotsList = GameObject.FindObjectsOfType<HideSpotController>();

        switch (gameMode)
        {
            case 0:
                GameObject.Find("hideSpots").SetActive(false);
                break;
            case 1:
                GameObject.Find("Traps").SetActive(false);
                break;
            case 2:
                StartCoroutine(gameTimeRoutine());
                break;
            case 3:
                GameObject.Find("Traps").SetActive(false);
                HideSuperSecretBagOfCandies();
                break;
        }
    }

    void Update()
    {
        if(gameMode == 0 || gameMode == 1)
        {
            if (kidCandies >= totalCandies || spookyCandies >= totalCandies)
            {
                ShowWinner("----GAME----\n" +
                    ((kidCandies >= totalCandies) ? ("Kid") : ("Spooky")) + " won!!");
            }
        }
        else if(gameMode == 3)
        {
            if (kidCandies >= SuperSecretBagRounds || spookyCandies >= SuperSecretBagRounds)
            {
                ShowWinner("----GAME----\n" +
                    ((kidCandies > spookyCandies) ? ("Kid") : ("Spooky")) + " won!!");
            }
        }

        if (Input.GetKeyDown("space"))
        {
            Vector3 position = new Vector3(
                GameObject.Find("Camera1").transform.position.x,
                GameObject.Find("Camera1").transform.position.y - 10, 0);

            GameObject newlogo = Instantiate(logo, position, Quaternion.identity);

            Vector3 position2 = new Vector3(
                GameObject.Find("Camera2").transform.position.x,
                GameObject.Find("Camera2").transform.position.y - 10, 0);

            GameObject newlogo2 = Instantiate(logo, position2, Quaternion.identity);
        }
    }

    public static void HideSuperSecretBagOfCandies()
    {
        int hideSpotWithSuperSecretBag = Random.Range(1, hideSpotsList.Length);
        hideSpotsList[hideSpotWithSuperSecretBag].
            GetComponent<HideSpotController>().hasSuperSecretBag = true;
        print("Is " + hideSpotsList[hideSpotWithSuperSecretBag].gameObject.name);
    }

    private IEnumerator gameTimeRoutine()
    {
        yield return new WaitForSeconds(totalMinutes * 60);
        ShowWinner("----GAME----\n" +
            ((kidCandies >= totalCandies) ? ("Kid") : ("Spooky")) + " won!!");
    }

    public void ShowWinner(string text)
    {
        newText = Instantiate(floatingText, BeginTextPosition.position,
                                       Quaternion.identity, BeginTextPosition);
        Renderer textRenderer = newText.GetComponent<Renderer>();
        textRenderer.sortingOrder = 6;
        newText.layer = 8;
        newText.GetComponent<TextMesh>().characterSize = 0.9f;
        newText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
        newText.GetComponent<FadeOut>().lifeTime = 0.9f;

        newText.GetComponent<TextMesh>().text = text;
        newText.GetComponent<FadeOut>().FadeInText();
    }
}
