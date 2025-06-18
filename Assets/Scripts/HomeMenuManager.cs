using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeMenuManager : MonoBehaviour
{
    //panels
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject aboutPanel;
    // buttons
    public Button startARButton;
    public Button settingsButton;
    public Button aboutButton;


    // Go to AR scene
    public void StartAR()
    {
        // Load the AR scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("ARFilterScene");
    }

    // Show settings panel
    public void ShowSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        aboutPanel.SetActive(false);

    }

    // Show about panel
    public void ShowAbout()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }
}
