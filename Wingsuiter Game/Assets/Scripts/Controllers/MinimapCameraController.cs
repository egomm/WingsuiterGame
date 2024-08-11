using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 10000, 0);

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the offset between the player and the minimap camera
        //offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the position of the minimap camera so that it is always above the player
        transform.position = player.transform.position + offset;
    }
}
