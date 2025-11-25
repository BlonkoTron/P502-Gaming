using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] PlayerSetUp playerSetUp;
    public GraphGenerator graph;

    [Header("Main Menu Panels")]
    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject selectArmPanel;
    [SerializeField] GameObject chooseConfigurationPanel;
    [SerializeField] GameObject manuelConfigurationPanel;
    [SerializeField] GameObject automaticConfigurationPanel;
    [SerializeField] GameObject startGamePanel;
    [SerializeField] GameObject statsPanel;

    [Header("Other GameObjects")]
    [SerializeField] Button startGameButton;
    [SerializeField] GameObject missingConfigText;


    private void Start()
    { 
        GoToPanel(welcomePanel);
        CheckIfConfigured();
    }

    public void ShowBents()
    {
        graph.showBents = true;
        graph.Redraw();
    }

    public void ShowRotations()
    {
        graph.showBents = false;
        graph.Redraw();
    }


    private void CheckIfConfigured()
    {
        if (playerSetUp.isConfigured)
        {
            startGameButton.interactable = true;
            missingConfigText.SetActive(false);
        }
        else
        {
            startGameButton.interactable = false;
            missingConfigText.SetActive(true);
            if (playerSetUp.bentROMIn!=0 && playerSetUp.bentROMOut!=0 && playerSetUp.rotationROMUp!=0 && playerSetUp.rotationROMDown!=0)
            {
                playerSetUp.isConfigured = true;
                startGameButton.interactable = true;
                missingConfigText.SetActive(false);
            }
        }
    }

    public void GoToPanel(GameObject panel)
    {         
        // Disable all panels
        welcomePanel.SetActive(false);
        selectArmPanel.SetActive(false);
        chooseConfigurationPanel.SetActive(false);
        manuelConfigurationPanel.SetActive(false);
        automaticConfigurationPanel.SetActive(false);
        startGamePanel.SetActive(false);
        statsPanel.SetActive(false);
        // Enable the selected panel
        panel.SetActive(true);
    }

    public void GoToCalibrationPanel()
    {
        if (playerSetUp.isManualConfig)
        {
            GoToPanel(manuelConfigurationPanel);
        }
        else
        {
            GoToPanel(automaticConfigurationPanel);
        }
    }


    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
