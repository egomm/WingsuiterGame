using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public float ConvertToRadians(float degrees)
    {
        return degrees * Mathf.PI / 180;
    }
}
