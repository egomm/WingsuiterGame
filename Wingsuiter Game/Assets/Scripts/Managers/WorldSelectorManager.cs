using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSelectorManager : MonoBehaviour
{
    /// <summary>
    /// Handle when the player selects a world.
    /// </summary>
    public void SelectWorld()
    {
        WorldListManager.HandleSelectedWorld(EventSystem.current.currentSelectedGameObject.name);
    }
}
