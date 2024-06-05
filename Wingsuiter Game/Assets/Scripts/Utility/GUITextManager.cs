using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUITextManager : MonoBehaviour
{
    public TMP_Text coordinateText;
    public TMP_Text speedText;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="movement"></param>
    /// <param name="changeInTime"></param>
    public void UpdateSpeedText(double velocity, double changeInTime)
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
}
