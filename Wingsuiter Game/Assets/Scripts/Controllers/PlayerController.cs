using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GUITextManager textManager;
    public Rigidbody rigidBody;
    public Utility utility;

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

        // Prevent the player from flying backward
        Debug.Log(verticalMovement);
        if (verticalMovement > 0)
        {
            //transform.Translate(Vector3.forward * Time.deltaTime * verticalMovement * 100;
            // Add the x magnitude to the movement vector
            //rigidBody.AddForce(100, 0, 0, ForceMode.Impulse
            Vector3 forwardDirection = transform.forward;
            forwardDirection.y = 0;
            rigidBody.AddForce(forwardDirection * 100, ForceMode.Impulse);
        }

        // Detect if the player presses the F key (to flare)
        if (Input.GetKeyDown(KeyCode.F))
        {
            float magX = Mathf.Sin(utility.ConvertToRadians(transform.rotation.eulerAngles.y));
            float magZ = Mathf.Cos(utility.ConvertToRadians(transform.rotation.eulerAngles.y));
            rigidBody.AddForce(new Vector3(magX, 1, magZ) * 5000, ForceMode.Impulse);
        }

        // Detect if the player moves their tail up/down
        float xChange = 0;
        if (Input.GetKey(KeyCode.S) && transform.rotation.eulerAngles.x >= 315 && transform.rotation.eulerAngles.x < 360)
        {
            xChange += 2f;
        }


        // Update the player position
        //transform.position = transform.position + movement;
        double velocity = rigidBody.velocity.magnitude;
        // Update the speed text
        textManager.UpdateSpeedText(velocity);

        // Rotate based on the horizontal movement
        //transform.Rotate(new Vector3(0, 50 * horizontalMovement * Time.deltaTime, horizontalMovement * Time.deltaTime * -100));
        
        // Get the change in the Z axis
        float zChange = horizontalMovement * Time.deltaTime * -40 * (Mathf.Abs(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180)) / 2 + 1.5f);
        // Get the change in y (depends on the change in z)
        float yChange = zChange * Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) / 2;

        if (transform.rotation.eulerAngles.z != 90)
        {
            yChange += -Mathf.Sin(2 * transform.rotation.eulerAngles.z * Mathf.PI / 180) / 5;
        }


        if (transform.rotation.eulerAngles.x > 315)
        {
            xChange -= 1f;
        }

        Debug.Log(xChange);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + xChange, transform.rotation.eulerAngles.y + (yChange), transform.rotation.eulerAngles.z + zChange);
        //-horizontalMovement * Time.deltaTime

        // Get the player transform
        Vector3 playerTransform = transform.position;
        textManager.UpdateCoordinateText(playerTransform);
    }
}
