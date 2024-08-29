using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    /// <summary>
    /// Convert from degree to radians.
    /// </summary>
    /// <param name="degrees">The angle in degrees</param>
    /// <returns>The angle in radians</returns>
    public float ConvertToRadians(float degrees)
    {
        return degrees * Mathf.PI / 180;
    }
}
