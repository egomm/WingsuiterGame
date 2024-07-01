using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Set the time (in seconds) between updates of the coin
    const float TIME_BETWEEN_UPDATE = 1;
    // Set the base coins per second
    // TODO: make the coins per second depend on the velocity
    const int COINS_PER_SECOND = 10;
    // Set the last update time
    float lastUpdateTime = 0;

    // Get the coin manager
    public GUITextManager textManager;

    // Get the average distance from the ground in the last second

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get the current time (in seconds)
        float currentTime = Time.time;
        // Check if the time since the last update is sufficient
        if ((currentTime - lastUpdateTime) > TIME_BETWEEN_UPDATE)
        {
            // Update the last update time
            lastUpdateTime = Time.time;
            // Update the coin text appropriately (based on the level of the coin multiplier)
            textManager.UpdateCoinText(COINS_PER_SECOND + DataManager.coinMultiplierLevel - 1);
        }
    }
}
