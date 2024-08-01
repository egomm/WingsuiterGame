using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldListManager : MonoBehaviour
{
    //
    public Button worldItem;
    public GameObject padding;
    public GameObject panel;


    public void HandleSelectWorld()
    {
        Debug.Log("Selected world");
    }


    // Start is called before the first frame update
    void Start()
    {
        // Iterate over the each world in the world list from the data manager
        //foreach (World world in DataManager.worldList)
        //{
        Debug.Log("Hello");
        //Instantiate(worldItem, new Vector3(0, 0, 0), Quaternion.identity);
        for (int i = 0; i < 10; i++)
        {
            Button newObj = Instantiate(worldItem);
            var paddingObj = Instantiate(padding);
            //newObj.transform.parent = GameObject.Find("Panel").transform;
            newObj.transform.SetParent(panel.transform);
            paddingObj.transform.SetParent(panel.transform);

            TextMeshProUGUI[] worldInformation = newObj.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI info in worldInformation)
            {
                if (info.CompareTag("World Title"))
                {
                    info.text = $"World {i}";
                }
                else if (info.CompareTag("Last Opened"))
                {
                    info.text = $"Last Opened: {i}";
                }
            }
        }
        //}
    }
}
