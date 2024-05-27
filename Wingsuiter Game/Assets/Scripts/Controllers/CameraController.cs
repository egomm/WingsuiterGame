using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the offset between the player and the camera
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position + offset;
        transform.eulerAngles = new Vector3(25, 270, 0);
    }
}
