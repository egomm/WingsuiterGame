using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameManager : MonoBehaviour
{
    public GameObject pausePanel;

    /// <summary>
    /// 
    /// </summary>
    public void PauseGame()
    {
        // Display the pause panel
        pausePanel.SetActive(true);
        // Freeze the game
        Time.timeScale = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResumeGame()
    {
        // Unfreeze the game
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
