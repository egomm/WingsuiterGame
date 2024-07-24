using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public Utility utility;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the offset between the player and the camera
        offset = transform.position - player.transform.position;
        transform.eulerAngles = new Vector3(25, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float offsetX = offset.x * Mathf.Sin(utility.ConvertToRadians(player.transform.rotation.eulerAngles.y));
        float offsetZ = offset.magnitude * -Mathf.Cos(utility.ConvertToRadians(player.transform.rotation.eulerAngles.y));

        float offsetY = 0;

        transform.position = player.transform.position + new Vector3(offsetX, offset.y + offsetY, offsetZ);

        
        transform.eulerAngles = new Vector3(25, player.transform.rotation.eulerAngles.y, 0);
    }
}
