using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldListLoader : MonoBehaviour
{
    public Button worldItem;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        // Iterate over the each world in the world list from the data manager
        //foreach (World world in DataManager.worldList)
        //{
            Debug.Log("Hello");
            //Instantiate(worldItem, new Vector3(0, 0, 0), Quaternion.identity);
        var newObj = Instantiate(worldItem);
        //newObj.transform.parent = GameObject.Find("Panel").transform;
        newObj.transform.SetParent(panel.transform);
        //}
    }
}
