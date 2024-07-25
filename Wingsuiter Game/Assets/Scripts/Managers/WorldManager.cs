using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WorldManager : MonoBehaviour
{
    private string worldNameInput;
    private int seedInput;

    /// <summary>
    /// 
    /// </summary>
    public void CreateWorld()
    {
    }

    public void ReadWorldName(string s)
    {

    }

    public void ReadSeedInput(string s)
    {
        try
        {
            seedInput = int.Parse(s);
            Debug.Log(seedInput);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OpenCreateWorldMenu()
    {
        SceneManager.LoadScene(sceneName: "World Creator");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ExitWorldSelector()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
