using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GUITextManager textManager;
    public Rigidbody rigidBody;

    // Declare the magnitude for the X movement
    const float xMagnitude = 1;
    // Declare the magnitude for the Y movement 
    const float yMagnitude = -0.1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(-45, 90, 90);
    }

    // Update is called once per frame at fixed intervals
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
            //transform.Translate(Vector3.forward * Time.deltaTime * verticalMovement * 100;
            // Add the x magnitude to the movement vector
            //rigidBody.AddForce(100, 0, 0, ForceMode.Impulse);
            Debug.Log(transform.forward);
            Vector3 forwardDirection = transform.forward;
            forwardDirection.y = 0;
            rigidBody.AddForce(forwardDirection * 100, ForceMode.Impulse);
        }

        // Update the player position
        //transform.position = transform.position + movement;
        double velocity = rigidBody.velocity.magnitude;
        // Update the speed text
        textManager.UpdateSpeedText(velocity, Time.deltaTime);

        // Rotate based on the horizontal movement
        //transform.Rotate(new Vector3(0, 50 * horizontalMovement * Time.deltaTime, horizontalMovement * Time.deltaTime * -100));
        
        // Get the change in the Z axis
        float zChange = horizontalMovement * Time.deltaTime * -40 * (Mathf.Abs(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180)) / 2 + 1.5f);
        // Get the change in y (depends on the change in z)
        float yChange = zChange * Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) / 2;

        transform.rotation = Quaternion.Euler(-45, transform.rotation.eulerAngles.y + (yChange), transform.rotation.eulerAngles.z + zChange);
        //-horizontalMovement * Time.deltaTime

        // Get the player transform
        Vector3 playerTransform = transform.position;
        textManager.UpdateCoordinateText(playerTransform);
    }
}
