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

    public void UpdateFlareText(int time, float delay)
    {
        // Calculate the green value
        float greenValue = (float) (delay - time) / delay;

        // Set the current colour
        Color currentColour = new Color(1, greenValue, 0.2f);
        if (time == 0) {
            flareText.text = "Flare: <color=\"green\">Ready</color>";
        } 
        else
        {
            flareText.text = $"Flare: <color=#{ColorUtility.ToHtmlStringRGBA(currentColour)}>{time}s</color>";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="movement"></param>
    /// <param name="changeInTime"></param>
    public void UpdateSpeedText(double velocity)
    {
        // Get the speed in kmph as an integer
        int speed = (int) (velocity * 3.6);
        // Update the speed text appropriately
        speedText.text = $"Speed: {speed} kmph";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerCoordinates"></param>
    public void UpdateCoordinateText(Vector3 playerCoordinates)
    {
        // Get the (x, y, z) coordinates of the player as integers
        int xPosition = (int)playerCoordinates.x;
        int yPosition = (int)playerCoordinates.y;
        int zPosition = (int)playerCoordinates.z;

        // Update the coordinates text with the player's current coordinates
        coordinateText.text = $"({xPosition}, {yPosition}, {zPosition})";
    }

    private int ClampRotation(int rotation)
    {
        if (rotation > 180)
        {
            return (rotation - 360);
        }
        return rotation;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerRotation"></param>
    public void UpdateRotationText(Vector3 playerRotation)
    {
        int xRotation = ClampRotation((int)playerRotation.x);
        int yRotation = ClampRotation((int)playerRotation.y);
        int zRotation = ClampRotation((int)playerRotation.z);
        rotationText.text = $"({xRotation}, {yRotation}, {zRotation})";
    }

    void ResetCoinTextSize()
    {
        coinText.fontSize = 20;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeInCoins"></param>
    public void UpdateCoinText(int changeInCoins)
    {
        // Update the coin amount
        DataManager.coinCount += changeInCoins;
        // Update the coin text
        coinText.text = DataManager.coinCount.ToString();
        // Temporarily increase the font size of the coinAmount
        coinText.fontSize = 22;
        // Set the font size back to normal in 0.1s
        Invoke("ResetCoinTextSize", 0.05f);
    }
}
