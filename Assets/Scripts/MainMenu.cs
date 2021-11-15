using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LocalMatch()
    {
        GameManager.room = 5;
        SceneManager.LoadScene("Lobby");
    }

    public void Training()
    {
        GameManager.room = 4;
        GameManager.gameMode = 4;
        SceneManager.LoadScene("GameScene");
    }
}
