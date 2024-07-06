using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        // Load the data if it hasn't already been loaded
        if (!DataManager.dataLoaded)
        {
            Debug.Log("CALLED");
            DataManager.LoadData();
        }
    }

    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene(sceneName: "Game");
    }

    public void UpgradeWingsuit()
    {
        SceneManager.LoadScene(sceneName: "Upgrade Menu");
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}
