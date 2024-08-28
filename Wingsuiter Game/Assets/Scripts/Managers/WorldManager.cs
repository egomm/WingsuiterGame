using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[Serializable]
public class Vector3S
{
    public float x;
    public float y;
    public float z;

    // A Vector3S at (0, 0, 0)
    public static readonly Vector3S zeroVector = new Vector3S(0, 0, 0);

    /// <summary>
    /// Constructor that initalises a new instance of the Vector3S class.
    /// </summary>
    /// <param name="x">The x coordinate</param>
    /// <param name="y">The y coordinate</param>
    /// <param name="z">The z coordinate</param>
    public Vector3S(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // Convert to Vector3
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    // Convert to Quaternion
    public Quaternion ToQuaternion()
    {
        return Quaternion.Euler(x, y, z);
    }
}

[Serializable]
public class World
{
    public string worldName;
    public int seed;
    public Vector3S spawnCoordinates;
    public Vector3S lastCoordinates;
    public Vector3S lastRotation;
    public float flareCooldown;
    public DateTime time;
    
    // Create a constructor for the world information
    public World(string worldName, int seed, Vector3S spawnCoordinates, Vector3S lastCoordinates, Vector3S lastRotation, float flareCooldown)
    {
        this.worldName = worldName;
        this.seed = seed;
        this.spawnCoordinates = spawnCoordinates;
        this.lastCoordinates = lastCoordinates;
        this.lastRotation = lastRotation;
        this.flareCooldown = flareCooldown;
        time = DateTime.Now;
    }
}

public class WorldManager : MonoBehaviour
{
    public GameObject errorPanel;
    public TMP_Text errorText;

    private string worldNameInput = "";
    private int seedInput;
    private bool seedFieldComplete = false;

    /// <summary>
    /// Load the World Creator scene
    /// </summary>
    public void OpenCreateWorldMenu()
    {
        SceneManager.LoadScene(sceneName: "World Creator");
    }

    /// <summary>
    /// Load the World Selector scene
    /// </summary>
    public void ExitWorldCreator()
    {
        SceneManager.LoadScene(sceneName: "World Selector");
    }

    /// <summary>
    /// Load the Main Menu scene
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
    /// Closes the error panel
    /// </summary>
    public void CloseErrorPanel()
    {
        errorPanel.SetActive(false);
    }

    /// <summary>
    /// Create a new world with the given world name and world seed
    /// </summary>
    public void CreateWorld()
    {
        // Generate a random seed if the seed field is not complete
        if (!seedFieldComplete)
        {
            // Get a seed value between 0 and the 32 bit integer limit (int max value)
            seedInput = UnityEngine.Random.Range(0, int.MaxValue);
        }

        // Trim the world name input
        worldNameInput = worldNameInput.Trim();

        // Check if the world name is more than 50 characters
        if (worldNameInput.Length > 50)
        {
            // Alert the player of the error
            errorText.text = "The world name must not exceed 50 characters in length.";
            // Show the panel
            errorPanel.SetActive(true);
        }
        else if (worldNameInput.Length == 0)
        {
            // Alert the player of the error
            errorText.text = "The world name is required.";
            // Show the panel
            errorPanel.SetActive(true);
        }
        else
        {
            bool worldNameInUse = false;

            // Iterate over the worlds to check if the world name is in use
            foreach (World world in DataManager.worldList)
            {
                if (world.worldName == worldNameInput)
                {
                    worldNameInUse = true;
                    break;
                }
            }

            if (worldNameInUse)
            {
                // Alert the player of the error
                errorText.text = $"The world name {worldNameInput} is already in use.";
                // Show the panel
                errorPanel.SetActive(true);
            }
            else
            {
                // Check if the seed field is too large
                if (seedFieldComplete)
                {
                    if (seedInput > int.MaxValue || seedInput < 0)
                    {
                        // Alert the player of the error
                        errorText.text = $"The seed must be between {0} and {int.MaxValue}";
                        // Show the panel
                        errorPanel.SetActive(true);
                    }
                }
                
                // Initalise a new world object with the world name and the seed
                World newWorld = new World(worldNameInput, seedInput, Vector3S.zeroVector, Vector3S.zeroVector, Vector3S.zeroVector, -1);
                // Add the new world to the list of worlds
                DataManager.worldList.Add(newWorld);

                // Sort the world list by the list opened time
                SortWorldList();

                // Exit the world creator after the user has created a new world
                ExitWorldCreator();
            }
        }
    }

    /// <summary>
    /// Edit the name of the world
    /// </summary>
    public void EditWorld()
    {
        // Flag to control whether the world was edited
        bool worldEdited = false;

        List<World> updatedWorldList = new List<World>();
        // Iterate over 
        foreach (World world in DataManager.worldList)
        {
            // Check if the world has been found
            if (world.worldName == DataManager.currentWorld.worldName)
            {
                // Trim the world name input
                worldNameInput = worldNameInput.Trim();

                // Check if the world name is more than 50 characters
                if (worldNameInput.Length > 50)
                {
                    // Alert the player of the error
                    errorText.text = "The world name must not exceed 50 characters in length.";
                    // Show the panel
                    errorPanel.SetActive(true);
                }
                else if (worldNameInput.Length == 0)
                {
                    // Alert the player of the error
                    errorText.text = "The world name is required";
                    // Show the panel
                    errorPanel.SetActive(true);
                }
                else 
                {
                    bool worldNameInUse = false;

                    // Iterate over the worlds to check if the world name is in use
                    foreach (World worldChecking in DataManager.worldList)
                    {
                        if (worldChecking.worldName == worldNameInput)
                        {
                            worldNameInUse = true;
                            break;
                        }
                    }

                    if (worldNameInUse)
                    {
                        // Alert the player of the error
                        errorText.text = $"The world name {worldNameInput} is already in use.";
                        // Show the panel
                        errorPanel.SetActive(true);
                    }
                    else
                    {
                        // Create an updated world
                        World updatedWorld = new World(worldNameInput, world.seed, world.spawnCoordinates, world.lastCoordinates, world.lastRotation, world.flareCooldown);
                        updatedWorldList.Add(updatedWorld);
                        worldEdited = true;
                    }
                }
            }
            else
            {
                updatedWorldList.Add(world);
            }
        }

        if (worldEdited)
        {
            // Update the world list
            DataManager.worldList = updatedWorldList;

            // Return back to the world selector scene
            SceneManager.LoadScene(sceneName: "World Selector");
        }
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
                Debug.LogError(ex.Message);
            }
        }
    }
}
