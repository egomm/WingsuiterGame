using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    /// <summary>
    /// Save the user data when the player quits the game.
    /// </summary>
    void OnApplicationQuit()
    {
        // Save the data
        DataManager.SaveData();
    }
}
