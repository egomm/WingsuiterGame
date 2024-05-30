using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text coordinateText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        if (horizontalMovement != 0)
        {
            Debug.Log($"Horizontal axis... {horizontalMovement}");
        }
        if (verticalMovement != 0)
        {
            Debug.Log($"Vertical axis... {verticalMovement}");
        }
        // Prevent the player from flying backward
        if (verticalMovement > 0)
        {
            //transform.Translate(Vector3.forward * Time.deltaTime * verticalMovement * 100);
            transform.position = transform.position + new Vector3(1, 0, 0);
        }

        // Rotate based on the horizontal movement
        transform.Rotate(new Vector3(0, 0, horizontalMovement * Time.deltaTime * -100));

        // Get the player transform
        Vector3 playerTransform = transform.position;
        // Get the (x, y, z) coordinates of the player as integers
        int xPosition = (int) playerTransform.x;
        int yPosition = (int) playerTransform.y;
        int zPosition = (int) playerTransform.z;

        // Update the coordinates text with the player's current coordinates
        coordinateText.text = $"({xPosition}, {yPosition}, {zPosition})";
    }
}
