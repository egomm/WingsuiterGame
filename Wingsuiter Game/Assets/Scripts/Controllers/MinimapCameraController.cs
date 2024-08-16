using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 10000, 0);
    public GameObject arrow;

    // Update is called once per frame
    void Update()
    {
        // Update the position of the minimap camera so that it is always above the player
        transform.position = player.transform.position + offset;

        // Update the arrow rotation
        arrow.transform.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.y);
    }
}
