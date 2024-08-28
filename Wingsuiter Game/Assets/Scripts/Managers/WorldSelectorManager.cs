using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldSelectorManager : MonoBehaviour
{
    public void SelectWorld()
    {
        WorldListManager.HandleSelectedWorld(EventSystem.current.currentSelectedGameObject.name);
    }
}
