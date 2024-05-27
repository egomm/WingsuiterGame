using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
            transform.Translate(Vector3.forward * Time.deltaTime * verticalMovement);
        }

        // Rotate based on the horizontal movement
        transform.Rotate(new Vector3(0, 0, horizontalMovement * Time.deltaTime * -100));
    }
}
