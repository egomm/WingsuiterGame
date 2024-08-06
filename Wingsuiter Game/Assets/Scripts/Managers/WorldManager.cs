using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[Serializable]
public class World
{
    public string worldName;
    public int seed;
    public DateTime time;
    
    // Create a constructor for the world information
    public World(string worldName, int seed)
    {
        this.worldName = worldName;
        this.seed = seed;
        time = DateTime.Now;
    }
}

// TODO: Add an option to delete a world

public class WorldManager : MonoBehaviour
{
    private string worldNameInput;
    private int seedInput;
    private bool seedFieldComplete = false;

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
    public void ExitWorldCreator()
    {
        SceneManager.LoadScene(sceneName: "World Selector");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ExitWorldSelector()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }

    /// <summary>
    /// Order the world list such that the most recently opened/created world is displayed first in the world list.
    /// This is done by comparing
    /// </summary>
    public static void SortWorldList()
    {
        // Sort the world by the last opened/created time
        DataManager.worldList.Sort((firstWorld, secondWorld) => DateTime.Compare(secondWorld.time, firstWorld.time));
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateWorld()
    {
        // Generate a random seed if the seed field is not complete
        if (!seedFieldComplete)
        {
            // Get a seed value between 0 and the 32 bit integer limit (int max value)
            seedInput = UnityEngine.Random.Range(0, int.MaxValue);
        }

        // Initalise a new world object with the world name and the seed
        World newWorld = new World(worldNameInput, seedInput);
        // Add the new world to the list of worlds
        DataManager.worldList.Add(newWorld);

        // Sort the world list by the list opened time
        SortWorldList();

        // Exit the world creator after the user has created a new world
        ExitWorldCreator();
    }

    /// <summary>
    /// Read the world name input from the world name input field
    /// </summary>
    /// <param name="input">The text in the world name input field</param>
    public void ReadWorldName(string input)
    {
        worldNameInput = input;
    }

    /// <summary>
    /// Read the seed input from the seed input field and parse it to an integer
    /// </summary>
    /// <param name="input">The text in the seed input field</param>
    public void ReadSeedInput(string input)
    {
        // Check that there a seed input
        if (input.Length > 0)
        {
            try
            {
                // Try to parse the seed input to an integer
                seedInput = int.Parse(input);
                // Set the seed field flag to true
                seedFieldComplete = true;
            }
            catch (Exception ex)
            {
                // Alert the user of the exception
                Debug.Log(ex.Message);
                // TODO: add a popup that alerts the user of the exception
            }
        }
    }
}
