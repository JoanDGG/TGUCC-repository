using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Traps;
    public GameObject hideSpots;

    public GameObject candies1;
    public GameObject candies2;
    public GameObject candies3;
    public static GameObject[] candies;

    public GameObject logo;
    
    public static List<GameObject> NPCNames = new List<GameObject>();
    public static int spookyCandies = 0;
    public static int kidCandies = 0;
    public static int gameMode = 3;
    /* 
     * Game Modes:
     * 0.- First to get 33 candies from scares!
     * 1.- First to get 33 candies from places!
     * 2.- Most candies in 6 minutes!
     * 3.- Find the super secret bag of candies first!
     * 
     */
    public static int totalCandies = 33;
    public static float totalMinutes = 6f;
    public static string whoFoundSuperSecretBag = "None";

    void Start()
    {
        GameObject gameObjectParent = GameObject.Find("NPCs");
        foreach (Transform t in gameObjectParent.transform)
        {
            NPCNames.Add(t.gameObject);
        }
        candies = new GameObject[] { candies1, candies2, candies3 };

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
                HideSpotController[] hideSpots = GameObject.
                    FindObjectsOfType<HideSpotController>();
                int hideSpotWithSuperSecretBag = Random.Range(1, hideSpots.Length);
                hideSpots[hideSpotWithSuperSecretBag].
                    GetComponent<HideSpotController>().hasSuperSecretBag = true;
                print("Is " + hideSpots[hideSpotWithSuperSecretBag].gameObject.name);
                break;
        }
    }

    void Update()
    {
        if(gameMode == 0 || gameMode == 1)
        {
            if (kidCandies >= totalCandies || spookyCandies >= totalCandies)
            {
                print("------------------GAME---------------------------");
                print(((kidCandies >= totalCandies) ? (kidCandies) : (spookyCandies)));
                print(((kidCandies >= totalCandies) ? ("Kid") : ("Spooky"))
                    + " won!!");
            }
        }
        else if(gameMode == 3)
        {
            if (whoFoundSuperSecretBag != "None")
            {
                print("------------------GAME---------------------------");
                print(whoFoundSuperSecretBag + " won!!");
            }
        }

        if (Input.GetKeyDown("space"))
        {
            Vector3 position = new Vector3(
                GameObject.Find("Main Camera").transform.position.x,
                GameObject.Find("Main Camera").transform.position.y, 0);

            position = Random.insideUnitCircle * 5;

            GameObject newlogo = Instantiate(logo, position, Quaternion.identity);
            
            logo.GetComponent<RandomLogo>().Aparecer();
        }
    }

    private IEnumerator gameTimeRoutine()
    {
        yield return new WaitForSeconds(totalMinutes * 60);
        print("------------------GAME---------------------------");
        print(((kidCandies >= totalCandies) ? ("Kid") : ("Spooky"))
            + " won!!");
    }
}
