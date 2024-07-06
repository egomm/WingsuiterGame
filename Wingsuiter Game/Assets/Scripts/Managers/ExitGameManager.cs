using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    void OnApplicationQuit()
    {
        Debug.Log("Exited");
        // Save the data
        DataManager.SaveData();
    }
}
