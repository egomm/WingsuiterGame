using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUITextManager : MonoBehaviour
{
    public TMP_Text coordinateText;
    public TMP_Text rotationText;
    public TMP_Text flareText;
    public TMP_Text speedText;
    public TMP_Text coinText;

    // Manage the size of the coin (normally and when it is small)
    private const int COIN_SIZE = 22;
    private const int SMALL_COIN_SIZE = 20;

    /// <summary>
    /// Update the flare text
    /// </summary>
    /// <param name="time"></param>
    /// <param name="delay"></param>
    public void UpdateFlareText(int time, float delay)
    {
        /* Calculate how 'green' the text colour should be. 
         * The strength of green in the text colour depends on the time until the flare is next available. */
        float greenValue = (float) (delay - time) / delay;

        // Set the current text colour for the flare text
        Color currentColour = new Color(1, greenValue, 0.2f);
        if (time == 0) {
            // Set the flare text colour to green when the flare is ready
            flareText.text = "Flare: <color=\"green\">Ready</color>";
        } 
        else
        {
            // Set the flare text colour to the calculate colour
            flareText.text = $"Flare: <color=#{ColorUtility.ToHtmlStringRGBA(currentColour)}>{time}s</color>";
        }
    }

    /// <summary>
    /// Update the speed text based on the player's velocity
    /// </summary>
    /// <param name="velocity">The player's velocity</param>
    public void UpdateSpeedText(double velocity)
    {
        // Get the speed in kmph as an integer
        int speed = (int) (velocity * 3.6);
        // Update the speed text appropriately
        speedText.text = $"Speed: {speed} kmph";
    }

    /// <summary>
    /// Update the coordinate text based on the player's coordinates
    /// </summary>
    /// <param name="playerCoordinates">The player's current position (coordinates)</param>
    public void UpdateCoordinateText(Vector3 playerCoordinates)
    {
        // Get the (x, y, z) coordinates of the player as integers
        int xPosition = (int)playerCoordinates.x;
        int yPosition = (int)playerCoordinates.y;
        int zPosition = (int)playerCoordinates.z;

        // Update the coordinates text with the player's current coordinates
        coordinateText.text = $"({xPosition}, {yPosition}, {zPosition})";
    }

    /// <summary>
    /// Clamp the rotation value between -180 and 180
    /// </summary>
    /// <param name="rotation">The rotation that is being clamped</param>
    /// <returns></returns>
    private int ClampRotation(int rotation)
    {
        if (rotation > 180)
        {
            return (rotation - 360);
        }
        return rotation;
    }

    /// <summary>
    /// Updates the rotation text based on the player rotation
    /// </summary>
    /// <param name="playerRotation">The player's current rotation</param>
    public void UpdateRotationText(Vector3 playerRotation)
    {
        /* Clamp the x, y, and z rotation so that the rotation is between -180 and 180
         * which is more intuitive for the users */
        int xRotation = ClampRotation((int)playerRotation.x);
        int yRotation = ClampRotation((int)playerRotation.y);
        int zRotation = ClampRotation((int)playerRotation.z);

        // Set the rotation text
        rotationText.text = $"({xRotation}, {yRotation}, {zRotation})";
    }

    /// <summary>
    /// Resets the size of the coin text
    /// </summary>
    void ResetCoinTextSize()
    {
        coinText.fontSize = SMALL_COIN_SIZE;
    }

    /// <summary>
    /// Load the coin text from the data manager
    /// </summary>
    public void LoadCoinText()
    {
        coinText.text = DataManager.coinCount.ToString();
    }

    /// <summary>
    /// Update the coin text by a change in coins
    /// </summary>
    /// <param name="changeInCoins">The change in coins (the amount the player earned)</param>
    public void UpdateCoinText(int changeInCoins)
    {
        // Update the coin amount
        DataManager.coinCount += changeInCoins;
        // Update the coin text
        coinText.text = DataManager.coinCount.ToString();
        // Temporarily increase the font size of the coinAmount
        coinText.fontSize = COIN_SIZE;
        // Set the font size back to normal in 0.1s
        Invoke("ResetCoinTextSize", 0.05f);
    }
}
