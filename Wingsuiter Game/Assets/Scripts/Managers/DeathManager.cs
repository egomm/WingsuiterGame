using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject deathPanel;
    public TMP_Text crashPenaltyText;
    public GUITextManager textManager;
    public GameObject player;

    public void OpenDeathPanel()
    {
        // Pause game
        deathPanel.SetActive(true);
        Time.timeScale = 0;

        int coinsLost = Mathf.RoundToInt(DataManager.coinCount * 0.25f);
        // Get the amount of coins the player lost (25%)
        crashPenaltyText.text = $"You lost {coinsLost} coins.";

        // Update the player's coin balance
        textManager.UpdateCoinText(-coinsLost);
    }

    public void CloseDeathPanel()
    {
        // Resume game
        deathPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Respawn()
    {
        // Set the player position to the world centre
        player.transform.position = new Vector3(0, 1500, 0);
        // Reset the player's rotation
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles.x, 90, 90);
        // Reset the flare countdown
        PlayerController.lastFlareTime = Time.time;

        // Close the death panel
        CloseDeathPanel();
    }

    public void ExitToMainMenu()
    {
        // Set the last saved position as the world centre
        // Set the last saved rotation as the default rotation

        // Change scenes to go to the main menu
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
