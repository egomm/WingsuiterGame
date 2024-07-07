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
}


public class DataManager : MonoBehaviour
{
    /* Constant values (Note: a const object is always static - https://stackoverflow.com/questions/408192/why-cant-i-have-public-static-const-string-s-stuff-in-my-class) */
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

    private static string dataDirectoryPath = "GameSaves";
    private static string dataFileName = "GameData";

    //public static Test test;

    // Save data
    public static void SaveData()
    {
        Debug.Log("Called");
        // Create directory if it does not already exist
        if (!Directory.Exists(dataDirectoryPath))
        {
            Directory.CreateDirectory(dataDirectoryPath);
        }

        // Initalise the binary formatter
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        // Get the file path (.bin format)
        FileStream binaryFile = File.Create(dataDirectoryPath + "/" + dataFileName + ".bin");

        GameData gameData = new GameData();
        gameData.coinCount = coinCount;
        gameData.coinMultiplierLevel = coinMultiplierLevel;
        gameData.movabilityLevel = movabilityLevel;
        gameData.flareCooldownLevel = flareCooldownLevel;

        // Seralise the data so that it can be written to the file
        binaryFormatter.Serialize(binaryFile, gameData);

        // Close the file
        binaryFile.Close();

        Debug.Log("Saved");
    }

    /// <summary>
    /// TODO: add try catch system/check if file exists
    /// </summary>
    public static void LoadData()
    {
        Debug.Log("Called");
        try
        {
            // Initalise the binary formatter
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            // Get the data file
            FileStream dataFile = File.Open(dataDirectoryPath + "/" + dataFileName + ".bin", FileMode.Open);

            // Deseralise the data to get the data
            GameData gameData = (GameData) binaryFormatter.Deserialize(dataFile);
            coinCount = gameData.coinCount;
            coinMultiplierLevel = gameData.coinMultiplierLevel;
            movabilityLevel = gameData.movabilityLevel;
            flareCooldownLevel = gameData.flareCooldownLevel;

            // Close the data file
            dataFile.Close();
        } 
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
