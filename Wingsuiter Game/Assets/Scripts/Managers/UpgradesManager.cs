using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UpgradesManager : MonoBehaviour
{
    // Panels for confirmations
    public GameObject coinPanel;
    public GameObject movabilityPanel;
    public GameObject flarePanel;

    // Coin text
    public TMP_Text coinText;

    // Upgrade buttons
    public Button coinUpgradeButton;
    public Button movabilityUpgradeButton;
    public Button flareUpgradeButton;

    // Text for the upgrade buttons
    public TMP_Text coinUpgradeText;
    public TMP_Text movabilityUpgradeText;
    public TMP_Text flareUpgradeText;

    // Text for the upgrade levels
    public TMP_Text coinLevelText;
    public TMP_Text movabilityLevelText;
    public TMP_Text flareLevelText;

    // Text for the upgrade confirmations
    public TMP_Text coinConfirmationText;
    public TMP_Text movabilityConfirmationText;
    public TMP_Text flareConfirmationText;

    // Text for the upgrade perks
    public TMP_Text coinUpgradePerks;
    public TMP_Text movabilityUpgradePerks;
    public TMP_Text flareUpgradePerks;

    // Upgrade costs
    private int coinMultiplierCost;
    private int movabilityCost;
    private int flareCooldownCost;

    // Colours for the button 
    // Note: need to divide by 255 since C# measures colour between 0 and 1
    private Color greenColour = new Color((float) 3 / 255, (float) 200 / 255, (float) 25 / 255);
    private Color redColour = new Color((float) 255 / 255, (float) 0 / 255, (float) 0 / 255);
    private Color goldColour = new Color((float) 255 / 255, (float) 215 / 255, (float) 0 / 255);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    private int CalculateUpgradeCost(int level)
    {
        // Calculate the upgrade cost, where C(n) = 125n^2 + 375n, where n is the current level and C(n) is the cost to upgrade
        int upgradeCost = 125 * (int) Mathf.Pow(level, 2) + 375 * level;
        return upgradeCost;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateUpgradeButtons()
    {
        // Update the balance text appropraitely
        coinText.text = DataManager.coinCount.ToString();

        // Upgrade the text for the upgrades
        coinLevelText.text = $"LEVEL {DataManager.coinMultiplierLevel}";
        movabilityLevelText.text = $"LEVEL {DataManager.movabilityLevel}";
        flareLevelText.text = $"LEVEL {DataManager.flareCooldownLevel}";

        // Update the text for the upgrade confirmations
        coinConfirmationText.text = $"Upgrade Coin Multiplier to Level {DataManager.coinMultiplierLevel + 1}";
        movabilityConfirmationText.text = $"Upgrade Movability to Level {DataManager.movabilityLevel + 1}";
        flareConfirmationText.text = $"Upgrade Flare Cooldown to Level {DataManager.flareCooldownLevel + 1}";

        // Calculate the current amount of coins per second and the amount at the following level
        int currentCoinsPerSecond = DataManager.BASE_COINS_PER_SECOND + (DataManager.coinMultiplierLevel - 1) * DataManager.ADDITIONAL_COINS_PER_SECOND;
        int nextCoinsPerSecond = currentCoinsPerSecond + DataManager.ADDITIONAL_COINS_PER_SECOND;

        // Calculate the current maximum speed multiplier and the multiplier at the following level
        float currentMaxSpeedMultiplier = DataManager.BASE_MAX_SPEED_MULTIPLIER + (DataManager.movabilityLevel - 1) * DataManager.ADDITIONAL_MAX_SPEED_MULTIPLIER;
        float nextMaxSpeedMultiplier = currentMaxSpeedMultiplier + DataManager.ADDITIONAL_MAX_SPEED_MULTIPLIER;

        // Calculate the current flare cooldown and the flare cooldown at the following level
        float currentFlareCooldown = DataManager.BASE_FLARE_COOLDOWN + (DataManager.flareCooldownLevel - 1) * DataManager.ADDITIONAL_FLARE_COOLDOWN;
        float nextFlareCooldown = currentFlareCooldown + DataManager.ADDITIONAL_FLARE_COOLDOWN;

        // Update the text for the perks
        coinUpgradePerks.text = $"{currentCoinsPerSecond} coins/second -> {nextCoinsPerSecond} coins/second";
        movabilityUpgradePerks.text = $"{currentMaxSpeedMultiplier:F2}x max speed -> {nextMaxSpeedMultiplier:F2}x max speed"; // Round to 2dp
        flareUpgradePerks.text = $"{currentFlareCooldown:F2} seconds -> {nextFlareCooldown:F2} seconds"; // Round to 2dp

        // Check if the user can afford each upgrade
        coinMultiplierCost = CalculateUpgradeCost(DataManager.coinMultiplierLevel);
        movabilityCost = CalculateUpgradeCost(DataManager.movabilityLevel);
        flareCooldownCost = CalculateUpgradeCost(DataManager.flareCooldownLevel);

        // Update the button text appropriately
        coinUpgradeText.text = coinMultiplierCost.ToString();
        movabilityUpgradeText.text = movabilityCost.ToString();
        flareUpgradeText.text = flareCooldownCost.ToString();

        // Disable all of the buttons by default
        Color coinUpgradeColour = redColour;
        coinUpgradeButton.enabled = false;
        Color movabilityUpgradeColour = redColour;
        movabilityUpgradeButton.enabled = false;
        Color flareUpgradeColour = redColour;
        flareUpgradeButton.enabled = false;

        // Check if the buttons should be enabled
        bool coinUpgradeEnabled = DataManager.coinCount >= coinMultiplierCost;
        bool movabilityUpgradeEnabled = DataManager.coinCount >= movabilityCost;
        bool flareUpgradeEnabled = DataManager.coinCount >= flareCooldownCost;

        // Check if the upgrade is at the maximum level
        bool coinUpgradeMax = DataManager.coinMultiplierLevel == DataManager.MAXIMUM_UPGRADE_LEVEL;
        bool movabilityUpgradeMax = DataManager.movabilityLevel == DataManager.MAXIMUM_UPGRADE_LEVEL;
        bool flareUpgradeMax = DataManager.flareCooldownLevel == DataManager.MAXIMUM_UPGRADE_LEVEL;

        // Enable that buttons that should be enabled
        if (coinUpgradeEnabled && !coinUpgradeMax)
        {
            coinUpgradeColour = greenColour;
            coinUpgradeButton.enabled = true;
        }

        if (movabilityUpgradeEnabled && !movabilityUpgradeMax)
        {
            movabilityUpgradeColour = greenColour;
            movabilityUpgradeButton.enabled = true;
        }

        if (flareUpgradeEnabled && !flareUpgradeMax)
        {
            flareUpgradeColour = greenColour;
            flareUpgradeButton.enabled = true;
        }

        // If upgrade is at the maximum level
        if (coinUpgradeMax)
        {
            coinUpgradeColour = goldColour;
            coinUpgradeText.text = "MAX";
        }

        if (movabilityUpgradeMax)
        {
            movabilityUpgradeColour = goldColour;
            movabilityUpgradeText.text = "MAX";
        }

        if (flareUpgradeMax)
        {
            flareUpgradeColour = goldColour;
            flareUpgradeText.text = "MAX";
        }

        // Update the colour of the buttons
        coinUpgradeButton.GetComponent<Image>().color = coinUpgradeColour;
        movabilityUpgradeButton.GetComponent<Image>().color = movabilityUpgradeColour;
        flareUpgradeButton.GetComponent<Image>().color = flareUpgradeColour;
    }

    void Start()
    {
        // 
        UpdateUpgradeButtons();
    }

    public void OpenCoinPanel()
    {
        // Display the coin panel
        coinPanel.SetActive(true);
    }

    public void CloseCoinPanel()
    {
        // Close the coin panel
        coinPanel.SetActive(false);
    }

    public void OpenMovabilityPanel()
    {
        // Display the movability panel
        movabilityPanel.SetActive(true);
    }

    public void CloseMovabilityPanel()
    {
        // Close the movability panel
        movabilityPanel.SetActive(false);
    }

    public void OpenFlarePanel()
    {
        // Open the flare panel
        flarePanel.SetActive(true);
    }

    public void CloseFlarePanel()
    {
        // Close the flare panel
        flarePanel.SetActive(false);
    }

    public void ExitUpgradeMenu()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }

    /// <summary>
    /// Note: This will only be called if the users balance is sufficient
    /// </summary>
    public void UpgradeCoinMultiplier()
    {
        // Subtract the cost from the user's balance
        DataManager.coinCount -= coinMultiplierCost;
        // Upgrade the coin multiplier
        DataManager.coinMultiplierLevel++;

        // Update the upgrade buttons
        UpdateUpgradeButtons();

        // Close the panel
        CloseCoinPanel();
    }

    /// <summary>
    /// Note: This will only be called if the users balance is sufficient
    /// </summary>
    public void UpgradeMovability()
    {
        // Subtract the cost from the user's balance
        DataManager.coinCount -= movabilityCost;
        // Upgrade the movability
        DataManager.movabilityLevel++;

        // Update the upgrade buttons
        UpdateUpgradeButtons();

        // Close the panel
        CloseMovabilityPanel();
    }

    /// <summary>
    /// Note: This will only be called if the users balance is sufficient
    /// </summary>
    public void UpgradeFlareCooldown()
    {
        // Subtract the cost from the user's balance
        DataManager.coinCount -= flareCooldownCost;
        // Upgrade the flare cooldown
        DataManager.flareCooldownLevel++;

        // Update the upgrade buttons
        UpdateUpgradeButtons();

        // Close the panel
        CloseFlarePanel();
    }
}
