using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InformationManager : MonoBehaviour
{
    /// <summary>
    /// Exit the information menu to the main menu.
    /// </summary>
    public void ExitInformationMenu()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
