using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    void OnApplicationQuit()
    {
        // Save the data
        DataManager.SaveData();
    }
}
