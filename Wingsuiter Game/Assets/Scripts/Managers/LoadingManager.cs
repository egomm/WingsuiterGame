using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Load the data 
        DataManager.LoadData();
        // Load the main menu scene
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
