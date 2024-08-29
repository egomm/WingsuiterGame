using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameManager : MonoBehaviour
{
    public GameObject pausePanel;

    /// <summary>
    /// Pause the game.
    /// </summary>
    public void PauseGame()
    {
        // Display the pause panel
        pausePanel.SetActive(true);
        // Freeze the game
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resume the game.
    /// </summary>
    public void ResumeGame()
    {
        // Unfreeze the game
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Exit the game.
    /// </summary>
    public void ExitGame()
    {
        // Unfreeze the game
        ResumeGame();
        // Load the main menu scene
        SceneManager.LoadScene(sceneName: "Main Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check to make sure that the pause game panel isn't active
            if (!pausePanel.activeSelf)
            {
                PauseGame();
            }
        }
    }
}
