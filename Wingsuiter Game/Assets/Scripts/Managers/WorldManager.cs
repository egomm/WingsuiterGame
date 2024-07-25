using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCreateWorldMenu()
    {
        SceneManager.LoadScene(sceneName: "World Creator");
    }

    public void ExitWorldSelector()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
