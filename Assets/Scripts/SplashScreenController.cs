using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] private float splashDuration = 3f;

    void Start()
    {
        Invoke("LoadMainMenu", splashDuration);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("HomePageScene");
    }
}