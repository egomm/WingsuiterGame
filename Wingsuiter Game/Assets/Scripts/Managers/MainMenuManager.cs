using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene(sceneName: "World Selector");
    }

    public void UpgradeWingsuit()
    {
        // Load the upgrade menu scene
        SceneManager.LoadScene(sceneName: "Upgrade Menu");
    }

    public void QuitGame()
    {
        // Quit the game
        // Note: the way that the game is quit varies on whether the game is in the editor or not
        // https://stackoverflow.com/questions/70437401/cannot-finish-the-game-in-unity-using-application-quit

        #if UNITY_STANDALONE
            Application.Quit();
        #endif

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
