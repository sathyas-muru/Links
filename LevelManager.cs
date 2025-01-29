using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadHowTo()
    {
        SceneManager.LoadScene("HowTo");
    }

    public void LoadMenue()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        print("Game quit");
        Application.Quit();
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}
