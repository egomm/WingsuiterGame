using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InformationManager : MonoBehaviour
{
    public void ExitInformationMenu()
    {
        Debug.Log("???");
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
