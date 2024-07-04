using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /* Constant values (Note: a const object is always static - https://stackoverflow.com/questions/408192/why-cant-i-have-public-static-const-string-s-stuff-in-my-class) */
    // Perks of each upgrade at the base level (1)
    public const int BASE_COINS_PER_SECOND = 10;
    public const float BASE_MAX_SPEED_MULTIPLIER = 1;
    public const float BASE_FLARE_COOLDOWN = 20;

    // Additional perks of each upgrade per level
    public const int ADDITIONAL_COINS_PER_SECOND = 1; // 110 coins per second at maximum level
    public const float ADDITIONAL_MAX_SPEED_MULTIPLIER = 0.04f; // 5x maximum speed at maximum level
    public const float ADDITIONAL_FLARE_COOLDOWN = -0.15f; // 5 second flare cooldown at maximum level

    // Maximum upgrade level (upgrades max out at level 101)
    public const int MAXIMUM_UPGRADE_LEVEL = 101;


    // Declare variables for the data so that they are accessible from any class (ie. are static)
    public static int coinCount = 0;
    public static int coinMultiplierLevel = 1;
    public static int movabilityLevel = 1;
    public static int flareCooldownLevel = 1;

}
