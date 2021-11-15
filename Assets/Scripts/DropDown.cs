using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    public Text description;
    public Text parameter;
    public InputField parameterInput;

    void Start()
    {
        HandleInputData(0);
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            description.text = "Trick Or Treat!\n" +
                "Oh sweet paradise of candies and scares!! " +
                "Just as classic as my sense of humor. This is, after all, the main reason " +
                "we're here for... \nThe goal, collect as many candies as possible! " +
                "The way? Scare people with traps! See who is the spookiest of them all! >:3";
            parameter.text = "Total candies";
            parameterInput.text = GameManager.totalCandies + "";
        }
        else if (val == 1)
        {
            description.text = "Candy Hunt!\n" +
                "Seems like old man MacQuoid isn't happy about our fun lil challenges " +
                "(something about heath attacks and 3rd degree burns?) but worry not, " +
                "cuz candy hunt is on! Look for different spots in the mansion for candies " +
                "and get them fast!";
            parameter.text = "Total candies";
            parameterInput.text = GameManager.totalCandies + "";
        }
        else if (val == 2)
        {
            description.text = "Rushdown!!\n" +
                "Time to create a little chaos around... Look for candies around the place, " +
                "scare people, you name it! Get ginormous amounts of candies until the time stop!" +
                "Then we'll see who the real master of spookies is! \\(@^0^@)/";
            parameter.text = "Total minutes";
            parameterInput.text = GameManager.totalMinutes + "";
        }
        else if (val == 3)
        {
            description.text = "Super Secret Bag of Candies!!\n" +
                "Now this. THIS is the real challenge! Theres only one bag of candies, " +
                "the ULTRA delicious Super Secret Bag of Candies ~(^¬^)~\n" +
                "The first to find a certain amount of times the bag wins!! " +
                "(we did multiple rounds cuz believe me, this matches wont " +
                "take too long!)";
            parameter.text = "Total rounds";
            parameterInput.text = GameManager.SuperSecretBagRounds + "";
        }
        GameManager.gameMode = val;
        print("Game Mode: " + GameManager.gameMode);
    }
}
