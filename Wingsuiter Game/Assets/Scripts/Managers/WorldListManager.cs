using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldListManager : MonoBehaviour
{
    //
    public Button worldItem;
    public GameObject padding;
    public GameObject panel;
    public GameObject loadingBackground;

    public List<GameObject> worldButtons = new List<GameObject>();

    // Create a dictionary to store information about the world buttons
    private static Dictionary<string, Button> worldButtonInformation = new Dictionary<string, Button>();
    // Manage the currently selected world
    private static string selectedWorldName;

    private static Color defaultWorldColour = new Color((float)240 / 255, (float)240 / 255, (float)240 / 255);
    private static Color selectedWorldColour = new Color((float)200 / 255, (float)200 / 255, (float)200 / 255);

    /// <summary>
    /// Open the selected world
    /// </summary>
    public void OpenWorld()
    {
        Debug.Log($"Selected world to open: {selectedWorldName}");

        // Find the world with this name
        foreach (World world in DataManager.worldList)
        {
            // Check if the world has been found
            if (world.worldName == selectedWorldName)
            {
                // Update the world appropriately
                DataManager.currentWorld = world;
                // Set the loading background to be visible
                loadingBackground.SetActive(true);
                // Switch to the game scene
                SceneManager.LoadScene(sceneName: "Game");

                // Exit the loop
                break;
            }
        }
    }

    /// <summary>
    /// Edit the selected world
    /// </summary>
    public void EditWorld()
    {
        // Find the world with this name
        foreach (World world in DataManager.worldList)
        {
            // Check if the world has been found
            if (world.worldName == selectedWorldName)
            {
                // Update the world appropriately
                DataManager.currentWorld = world;
                // Switch to the edit scene 
                SceneManager.LoadScene(sceneName: "World Editor");

                // Exit the loop
                break;
            }
        }
    }

    /// <summary>
    /// Handle the selection of a world (ie. change the world colour)
    /// </summary>
    /// <param name="worldName">The name of the world selected</param>
    public static void HandleSelectedWorld(string worldName)
    {
        // Ensure that this world name is valid
        if (worldButtonInformation.ContainsKey(worldName))
        {
            // Unselect the currently selected world
            if (selectedWorldName != null)
            {
                Button selectedButton = worldButtonInformation[selectedWorldName];
                selectedButton.image.color = defaultWorldColour;
            }

            // Select the clicked world
            Button newSelectedButton = worldButtonInformation[worldName];
            newSelectedButton.image.color = selectedWorldColour;

            // Update the selected world name
            selectedWorldName = worldName;
        }
    }

    /// <summary>
    /// Update the world list with the worlds from the data manager
    /// </summary>
    public void UpdateWorldList()
    {
        // Clear the current world information
        worldButtonInformation.Clear();
        selectedWorldName = null;
        foreach (GameObject button in worldButtons)
        {
            Destroy(button);
        }

        foreach (World world in DataManager.worldList)
        {
            Button worldObj = Instantiate(worldItem);
            worldButtons.Add(worldObj.gameObject);

            // Set the name of the world object
            worldObj.name = world.worldName;

            //var paddingObj = Instantiate(padding);
            //newObj.transform.parent = GameObject.Find("Panel").transform;
            worldObj.transform.SetParent(panel.transform, false);
            //paddingObj.transform.SetParent(panel.transform, false);

            // Add this world information to the dictionary
            worldButtonInformation.Add(worldObj.name, worldObj);

            TextMeshProUGUI[] worldInformation = worldObj.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI info in worldInformation)
            {
                if (info.CompareTag("World Title"))
                {
                    info.text = world.worldName;
                }
                else if (info.CompareTag("Last Opened"))
                {
                    info.text = $"Last Opened: {world.time}";
                }
            }
        }

        // Select the first world
        if (DataManager.worldList.Count > 0)
        {
            string firstWorld = DataManager.worldList[0].worldName;
            Button selectedButton = worldButtonInformation[firstWorld];
            selectedButton.image.color = selectedWorldColour;
            selectedWorldName = firstWorld;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateWorldList();
    }

    /// <summary>
    /// Delete the world
    /// </summary>
    public void DeleteWorld()
    {
        List<World> updatedWorldList = new List<World>();
        // Iterate over 
        foreach (World world in DataManager.worldList)
        {
            // Check if the world has been found
            if (world.worldName == selectedWorldName)
            {
                // Skip it
                continue;
            }
            else
            {
                updatedWorldList.Add(world);
            }
        }

        // Update the world list
        DataManager.worldList = updatedWorldList;
        UpdateWorldList();
    }
}
