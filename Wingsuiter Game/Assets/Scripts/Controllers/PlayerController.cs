using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public GUITextManager textManager;
    public DeathManager deathManager;
    public Rigidbody rigidBody;
    public Utility utility;
    public GameObject loadingBackground;

    // Declare the magnitude for the X movement
    private const float xMagnitude = 1;
    // Declare the magnitude for the Y movement 
    private const float yMagnitude = -0.1f;

    // Time between each flare
    private float FLARE_DELAY = DataManager.BASE_FLARE_COOLDOWN + ((DataManager.flareCooldownLevel - 1) * DataManager.ADDITIONAL_FLARE_COOLDOWN);

    // Manage the last time the flare was fired
    public static float lastFlareTime = 0;

    public static float groundDistance = float.MaxValue;

    // Make a guess for the world spawn coordinates 
    private Vector3 guessCoordinates = new Vector3(0, 1250, 0);

    private int i = 0;


    /// <summary>
    /// Calculates the closest (ie. smallest) distance to the ground from a given position.
    /// </summary>
    /// <param name="startPos">The position to calculate the distance from</param>
    /// <returns></returns>
    private float ClosestDistanceToGround(Vector3 startPos)
    {
        MeshCollider[] meshColliders = FindObjectsOfType<MeshCollider>();

        MeshCollider closestCollider = null;

        // Store the distance of the closest mesh collider and vertex
        float closestDistance = float.MaxValue;

        // Find the closest mesh collider
        foreach (MeshCollider meshCollider in meshColliders)
        {
            if (meshCollider.name != "Terrain Chunk") continue;

            float distance = Vector3.Distance(startPos, meshCollider.transform.position);
            if (distance < closestDistance)
            {
                // Update the new closest collider
                closestCollider = meshCollider;
                closestDistance = distance;
            }
        }

        if (closestCollider != null)
        {
            // Access the mesh of the closest mesh collider
            Mesh mesh = closestCollider.sharedMesh;

            if (mesh != null)
            {
                closestDistance = float.MaxValue;

                // Iterate through each vertex in the mesh
                foreach (Vector3 vertex in mesh.vertices)
                {
                    // Convert vertex from local space to world space
                    Vector3 worldVertex = closestCollider.transform.TransformPoint(vertex);

                    // Calculate the distance from the player to the vertex
                    float distance = Vector3.Distance(startPos, worldVertex);

                    // Check if this is the closest vertex
                    if (distance < closestDistance)
                    {
                        // Update the closest distance
                        closestDistance = distance;
                        groundDistance = distance;
                    }
                }

                return closestDistance;
            }
        }

        return -1;
    }

    public void UpdateLastPosition()
    {
        // Update the world information
        foreach (World world in DataManager.worldList)
        {
            // Check if the current iteration matches the current world
            if (DataManager.currentWorld.worldName == world.worldName)
            {
                Vector3 rotationEuler = transform.rotation.eulerAngles;
                Vector3S seralisedLastPosition = new Vector3S(transform.position.x, transform.position.y, transform.position.z);
                Vector3S seralisedLastRotation = new Vector3S(rotationEuler.x, rotationEuler.y, rotationEuler.z);
                // Update the last coordinates
                DataManager.currentWorld.lastCoordinates = seralisedLastPosition;
                world.lastCoordinates = seralisedLastPosition;
                world.lastRotation = seralisedLastRotation;

                // Break from the loop
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");

        Vector3 lastSavedCoordinates = DataManager.currentWorld.lastCoordinates.ToVector3();
        Vector3 worldSpawnCoordinates = DataManager.currentWorld.spawnCoordinates.ToVector3();
        Vector3 rotationVector = DataManager.currentWorld.lastRotation.ToVector3();
        Quaternion lastRotation = DataManager.currentWorld.lastRotation.ToQuaternion();

        if (worldSpawnCoordinates == Vector3.zero)
        {
            DataManager.gameRunning = false;
            loadingBackground.SetActive(true);
        }
        else
        {
            DataManager.gameRunning = true;
            // Check if there is a last saved position
            if (lastSavedCoordinates != Vector3.zero)
            {
                transform.position = lastSavedCoordinates;
            }
            else
            {
                transform.position = worldSpawnCoordinates;
            }

            if (rotationVector != Vector3.zero)
            {
                transform.rotation = lastRotation;
            }
            else
            {
                transform.rotation = DataManager.BASE_ROTATION.ToQuaternion();
            }
            // Get the current time
            float currentTime = Time.time;
            // Update the last flare time (with floor to prevent minor loading issues)
            lastFlareTime = Mathf.Floor(currentTime);
        }
    }

    // Update is called once per frame at fixed intervals
    void FixedUpdate()
    {
        if (DataManager.gameRunning)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            // Detect if the player moves their tail up/down
            float xChange = 0;

            if (transform.rotation.eulerAngles.x >= 316 || transform.rotation.eulerAngles.x <= 0)
            {
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
            rigidBody.AddForce(forwardDirection * 500 * speedMultiplier, ForceMode.Impulse);

            float currentTime = Time.time;
            float timeSinceFlare = currentTime - lastFlareTime;

            // Update flare text 
            if (timeSinceFlare < FLARE_DELAY)
            {
                int nextFlareTime = (int)Mathf.Ceil(FLARE_DELAY - timeSinceFlare);
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
                yChange += Mathf.Sin(2 * transform.rotation.eulerAngles.z * Mathf.PI / 180) / 3;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + xChange, transform.rotation.eulerAngles.y + (yChange), transform.rotation.eulerAngles.z + zChange);

            // Get the player transform
            Vector3 playerTransform = transform.position;
            textManager.UpdateCoordinateText(playerTransform);
            Vector3 playerRotation = transform.rotation.eulerAngles;
            textManager.UpdateRotationText(playerRotation);

            float distanceToGround = ClosestDistanceToGround(playerTransform);

            if (distanceToGround >= 0)
            {
                // Update the ground text
                textManager.UpdateGroundText(distanceToGround);
            }

            UpdateLastPosition();
        }
        else
        {
            i++;
            if (i % 10 == 0)
            {
                bool hasChunk = false;
                MeshCollider[] meshColliders = FindObjectsOfType<MeshCollider>();

                foreach (MeshCollider meshCollider in meshColliders)
                {
                    if (meshCollider.name == "Terrain Chunk")
                    {
                        hasChunk = true;
                        break;
                    }
                }

                if (hasChunk)
                {
                    Debug.Log("Found chunk");

                    float groundDistance = ClosestDistanceToGround(guessCoordinates);
                    Debug.Log(guessCoordinates.y);
                    Debug.Log(groundDistance);

                    // Find adequate world spawn coordinates (must be at least 500m from the ground)
                    if (groundDistance < 500)
                    {
                        // Increase the guess Y coordinate by 250
                        guessCoordinates.y += 250;
                    }
                    else
                    {
                        // Set the world spawn coordinates to the adequate coordinates
                        Vector3 worldSpawnCoordinates = guessCoordinates;

                        // Set the player's position to the world spawn
                        transform.position = worldSpawnCoordinates;

                        // Start game
                        lastFlareTime = Time.time;
                        DataManager.gameRunning = true;
                        loadingBackground.SetActive(false);

                        // Update the world information
                        foreach (World world in DataManager.worldList)
                        {
                            // Check if the current iteration matches the current world
                            if (DataManager.currentWorld.worldName == world.worldName)
                            {
                                Vector3S seralisedWorldSpawn = new Vector3S(worldSpawnCoordinates.x, worldSpawnCoordinates.y, worldSpawnCoordinates.z);
                                // Update the world's spawn coordinates
                                DataManager.currentWorld.spawnCoordinates = seralisedWorldSpawn;
                                DataManager.currentWorld.lastCoordinates = seralisedWorldSpawn;
                                world.spawnCoordinates = seralisedWorldSpawn;
                                world.lastCoordinates = seralisedWorldSpawn;

                                // Break from the loop
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (DataManager.gameRunning)
        {
            deathManager.OpenDeathPanel();
        }
    }
}
