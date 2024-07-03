using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /* Constant values */
    // Perks of each upgrade at the base level (1)
    public static int baseCoinsPerSecond = 10;
    public static float baseMaxSpeedMultiplier = 1;
    public static float baseFlareCooldown = 20;

    // Additional perks of each upgrade per level
    public static int additionalCoinsPerSecond = 1; // 110 coins per second at maximum level
    public static float additionalMaxSpeedMultiplier = 0.04f; // 5x maximum speed at maximum level
    public static float additionalFlareCooldown = 0.15f; // 5 second flare cooldown at maximum level

    // Maximum upgrade level (upgrades max out at level 101)
    public static int maximumUpgradeLevel = 101;


    // Declare variables for the data so that they are accessible from any class (ie. are static)
    public static int coinCount = 0;
    public static int coinMultiplierLevel = 1;
    public static int movabilityLevel = 1;
    public static int flareCooldownLevel = 1;

}
