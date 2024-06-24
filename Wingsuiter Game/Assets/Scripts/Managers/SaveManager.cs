using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    // Constructor for managing the saving of the files
    public SaveManager(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    // Load the data
    /*public GameData LoadData()
    {

    }*/

    // Save data
    public void SaveData(string data)
    {
        // Get the path of the file from the directory + the file name
        string filePath = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            // Get the directory name
            string directoryName = Path.GetDirectoryName(filePath);

            // Create the directory (if it doesn't already exist)
            Directory.CreateDirectory(directoryName);

            // Convert the data to JSON
            string jsonData = JsonUtility.ToJson(data);

            // Write the data to the data file
            FileStream stream = File.Create(filePath);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(jsonData);
        } 
        catch (Exception ex)
        {
            Debug.LogError($"Error on saving path to file: {ex}");
        }
    }
}
