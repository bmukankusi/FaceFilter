using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonController : MonoBehaviour
{
    //Panels
    public GameObject MainPanel;
    public GameObject SettingsPanel;
    public GameObject AboutPanel;

    public void BackToHomePage()
    {
        SettingsPanel.SetActive(false);
        AboutPanel.SetActive(false);
        // Show the main panel
        MainPanel.SetActive(true);
    }

    //Back to HomePageScene
    public void BackToHomePageScene()
    {
        // Load the HomePage scene
        SceneManager.LoadScene("HomePageScene");
    }
}
