using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GUITextManager textManager;
    public Rigidbody rigidBody;
    public Utility utility;
    public WorldGenerator worldGenerator;

    // Declare the magnitude for the X movement
    private const float xMagnitude = 1;
    // Declare the magnitude for the Y movement 
    private const float yMagnitude = -0.1f;

    // Time between each flare
    private float FLARE_DELAY = DataManager.BASE_FLARE_COOLDOWN + ((DataManager.flareCooldownLevel - 1) * DataManager.ADDITIONAL_FLARE_COOLDOWN);

    // Manage the last time the flare was fired
    private float lastFlareTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(-45, 90, 90);
        // Get the current time
        float currentTime = Time.time;
        // Update the last flare time (with floor to prevent minor loading issues)
        lastFlareTime = Mathf.Floor(currentTime);

        //
        //worldGenerator.GenerateWorldData();
    }

    // Update is called once per frame at fixed intervals
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Detect if the player moves their tail up/down
        float xChange = 0;

        if (transform.rotation.eulerAngles.x >= 316 || transform.rotation.eulerAngles.x <= 0) {
            xChange += verticalMovement;
        } 
        else if (transform.rotation.eulerAngles.x >= 0 && transform.rotation.eulerAngles.x < 180 && verticalMovement < 0)
        {
            xChange += verticalMovement;
        }
        else if (transform.rotation.eulerAngles.x <= 316 && transform.rotation.eulerAngles.x > 180 && verticalMovement > 0)
        {
            xChange += verticalMovement;
        }

        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0;
        // Speed is adjusted for the movability level
        float speedMultiplier = 1 + (DataManager.movabilityLevel - 1) * 0.04f;
        // Move the player (the player should move irrespective of whether they are pressing forward or not)
        rigidBody.AddForce(forwardDirection * 100 * speedMultiplier, ForceMode.Impulse);

        float currentTime = Time.time;
        float timeSinceFlare = currentTime - lastFlareTime;

        // Update flare text 
        if (timeSinceFlare < FLARE_DELAY)
        {
            int nextFlareTime = (int) Mathf.Ceil(FLARE_DELAY - timeSinceFlare);
            textManager.UpdateFlareText(nextFlareTime, FLARE_DELAY);
        }
        else
        {
            textManager.UpdateFlareText(0, FLARE_DELAY);
        }

        // Detect if the player presses the F key (to flare)
        if (Input.GetKeyDown(KeyCode.F) && timeSinceFlare > FLARE_DELAY)
        {
            lastFlareTime = Time.time;
            float magX = Mathf.Sin(utility.ConvertToRadians(transform.rotation.eulerAngles.y));
            float magZ = Mathf.Cos(utility.ConvertToRadians(transform.rotation.eulerAngles.y));
            rigidBody.AddForce(new Vector3(magX, 1, magZ) * 20000, ForceMode.Impulse);
        }

        // Update the player position
        double velocity = Mathf.Sqrt(Mathf.Pow(rigidBody.velocity.x, 2) + Mathf.Pow(rigidBody.velocity.z, 2));
        // Update the speed text
        textManager.UpdateSpeedText(velocity);
        
        // Get the change in the Z axis
        float zChange = horizontalMovement * Time.deltaTime * -40 * (Mathf.Abs(Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180)) / 2 + 1.5f);
        // Get the change in y (depends on the change in z)
        float yChange = zChange * Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.PI / 180) / 2;

        if (transform.rotation.eulerAngles.z != 90)
        {
            yChange += Mathf.Sin(2 * transform.rotation.eulerAngles.z * Mathf.PI / 180) / 5;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + xChange, transform.rotation.eulerAngles.y + (yChange), transform.rotation.eulerAngles.z + zChange);

        // Get the player transform
        Vector3 playerTransform = transform.position;
        textManager.UpdateCoordinateText(playerTransform);
        Vector3 playerRotation = transform.rotation.eulerAngles;
        textManager.UpdateRotationText(playerRotation);
    }
}
