using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public struct GameData
{
    public int coinCount;
    public int coinMultiplierLevel;
    public int movabilityLevel;
    public int flareCooldownLevel;
    public List<World> worldList;
}

public class DataManager 
{
    // Base rotation
    public static Vector3S BASE_ROTATION = new Vector3S(-45, 90, 90);

    /* Constant values (Note: a const object is always static) */
    // Perks of each upgrade at the base level (1)
    public const int BASE_COINS_PER_SECOND = 10;
    public const float BASE_MAX_SPEED_MULTIPLIER = 1;
    public const float BASE_FLARE_COOLDOWN = 20;

    // Additional perks of each upgrade per level
    public const int ADDITIONAL_COINS_PER_SECOND = 1; // 110 coins per second at maximum level
    public const float ADDITIONAL_MAX_SPEED_MULTIPLIER = 0.04f; // 5x maximum speed at maximum level
    public const float ADDITIONAL_FLARE_COOLDOWN = -0.15f; // 5 second flare cooldown at maximum level

    // Maximum upgrade level (upgrades max out at level 101)
    public const int MAXIMUM_UPGRADE_LEVEL = 101;

    // Declare variables for the data so that they are accessible from any class (ie. are static)
    public static int coinCount = 0;
    public static int coinMultiplierLevel = 1;
    public static int movabilityLevel = 1;
    public static int flareCooldownLevel = 1;
    public static List<World> worldList = new List<World>();
    public static World currentWorld = null;
    public static bool gameRunning = false;

    private static string dataDirectoryPath = "GameSaves";
    private static string dataFileName = "GameData";

    /// <summary>
    /// Save the user data to the binary data file
    /// </summary>
    public static void SaveData()
    {
        // Create directory if it does not already exist
        if (!Directory.Exists(dataDirectoryPath))
        {
            Directory.CreateDirectory(dataDirectoryPath);
        }

        // Initalise the binary formatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        // Get the file path (.bin format)
        // Note: this overrides the existing binary file
        // TODO: this may not be adequate in the future
        FileStream binaryFile = File.Create(dataDirectoryPath + "/" + dataFileName + ".bin");

        // Create a new game data object
        GameData gameData = new GameData();
        // Add the player's data to the game data object
        gameData.coinCount = coinCount;
        gameData.coinMultiplierLevel = coinMultiplierLevel;
        gameData.movabilityLevel = movabilityLevel;
        gameData.flareCooldownLevel = flareCooldownLevel;
        gameData.worldList = worldList;

        // Seralise the data so that it can be written to the file
        binaryFormatter.Serialize(binaryFile, gameData);

        // Close the file
        binaryFile.Close();
    }

    /// <summary>
    /// Load the player data from the binary data file
    /// </summary>
    public static void LoadData()
    {
        try
        {
            Debug.Log("Loading world data");

            // Initalise the binary formatter
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            // Get the data file
            FileStream dataFile = File.Open(dataDirectoryPath + "/" + dataFileName + ".bin", FileMode.OpenOrCreate);

            // Deseralise the data to get the data
            GameData gameData = (GameData) binaryFormatter.Deserialize(dataFile);
            // Close the data file
            dataFile.Close();

            coinCount = gameData.coinCount;
            coinMultiplierLevel = gameData.coinMultiplierLevel;
            movabilityLevel = gameData.movabilityLevel;
            flareCooldownLevel = gameData.flareCooldownLevel;
            worldList = gameData.worldList;

            // Sort the world list by the last opened time
            WorldManager.SortWorldList();
        } 
        catch (Exception ex)
        {
            // Log the exception
            Debug.LogError($"Error on loading path from file: {ex}");
        }
    }
}
