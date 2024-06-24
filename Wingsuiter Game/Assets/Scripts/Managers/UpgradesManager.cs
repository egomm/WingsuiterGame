using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradesManager : MonoBehaviour
{

    public GameObject coinPanel;
    public GameObject movabilityPanel;
    public GameObject flarePanel;

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
}
