using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] PlayerSetUp playerSetUp;
    public GraphGenerator graph;

    [Header("Main Menu Panels")]
    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject calibrationPanel;

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


    public void CheckIfConfigured()
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
        statsPanel.SetActive(false);
        calibrationPanel.SetActive(false);
        // Enable the selected panel
        panel.SetActive(true);
    }

    public void StartDelayGoToPanel(GameObject panel)
    {
        StartCoroutine (DelayGoToPanel(0.5f, panel));
    }

    // this needs to be changed when we have multiple scenes
    public void StartGame()
    {
        if(playerSetUp.bentROMOut < 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else if (playerSetUp.bentROMOut > 180)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    IEnumerator DelayGoToPanel(float delay, GameObject panel)
    {
        // Disable all panels
        welcomePanel.SetActive(false);
        statsPanel.SetActive(false);
        calibrationPanel.SetActive(false);

        yield return new WaitForSeconds(delay);

        panel.SetActive(true);

    }

    

}
