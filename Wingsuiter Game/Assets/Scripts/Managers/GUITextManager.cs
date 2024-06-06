using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUITextManager : MonoBehaviour
{
    public TMP_Text coordinateText;
    public TMP_Text speedText;
    public TMP_Text coinText;

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
        // Ensure that the coinText is not empty
        if (coinText.text.Length > 0)
        {
            // Get the current amount of coins
            int coinAmount = int.Parse(coinText.text);
            // Update the coin amount
            coinAmount += changeInCoins;
            // Update the coin text
            coinText.text = coinAmount.ToString();
            // Temporarily increase the font size of the coinAmount
            coinText.fontSize = 22;
            // Set the font size back to normal in 0.1s
            Invoke("ResetCoinTextSize", 0.05f);
        }
    }
}
