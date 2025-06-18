using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Audio References")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string backgroundMusicParameter = "BackgroundMusic";
    [SerializeField] private string buttonSoundsParameter = "ButtonSounds";

    [Header("UI References")]
    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider buttonSoundsSlider;
    [SerializeField] private TMP_Dropdown graphicsQualityDropdown;
    [SerializeField] private Toggle muteToggle;

    private bool isMuted = false;
    private float preMuteMusicVolume;
    private float preMuteSoundsVolume;

    private void Awake()
    {
        // Load saved settings or set defaults
        LoadSettings();

        // Setup listeners for UI changes
        if (backgroundMusicSlider != null)
        {
            backgroundMusicSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
        }

        if (buttonSoundsSlider != null)
        {
            buttonSoundsSlider.onValueChanged.AddListener(SetButtonSoundsVolume);
        }

        if (graphicsQualityDropdown != null)
        {
            graphicsQualityDropdown.onValueChanged.AddListener(SetGraphicsQuality);
        }

        if (muteToggle != null)
        {
            muteToggle.onValueChanged.AddListener(ToggleMute);
        }
    }

    private void LoadSettings()
    {
        // Audio settings
        float bgMusicVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.75f);
        float buttonSoundsVolume = PlayerPrefs.GetFloat("ButtonSoundsVolume", 0.75f);

        SetBackgroundMusicVolume(bgMusicVolume);
        SetButtonSoundsVolume(buttonSoundsVolume);

        if (backgroundMusicSlider != null)
        {
            backgroundMusicSlider.value = bgMusicVolume;
        }

        if (buttonSoundsSlider != null)
        {
            buttonSoundsSlider.value = buttonSoundsVolume;
        }

        // Graphics quality
        int qualityLevel = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(qualityLevel);

        if (graphicsQualityDropdown != null)
        {
            graphicsQualityDropdown.value = qualityLevel;
        }

        // Mute state
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        if (muteToggle != null)
        {
            muteToggle.isOn = isMuted;
        }

        if (isMuted)
        {
            // Store pre-mute volumes
            preMuteMusicVolume = bgMusicVolume;
            preMuteSoundsVolume = buttonSoundsVolume;

            // Apply mute
            audioMixer.SetFloat(backgroundMusicParameter, -80f);
            audioMixer.SetFloat(buttonSoundsParameter, -80f);
        }
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        if (isMuted) return;

        // Convert linear slider value to logarithmic dB value
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(backgroundMusicParameter, dB);
        PlayerPrefs.SetFloat("BackgroundMusicVolume", volume);
    }

    public void SetButtonSoundsVolume(float volume)
    {
        if (isMuted) return;

        // Convert linear slider value to logarithmic dB value
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat(buttonSoundsParameter, dB);
        PlayerPrefs.SetFloat("ButtonSoundsVolume", volume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        PlayerPrefs.SetInt("IsMuted", mute ? 1 : 0);

        if (mute)
        {
            // Store current volumes before muting
            preMuteMusicVolume = backgroundMusicSlider != null ? backgroundMusicSlider.value : 0.75f;
            preMuteSoundsVolume = buttonSoundsSlider != null ? buttonSoundsSlider.value : 0.75f;

            // Apply mute
            audioMixer.SetFloat(backgroundMusicParameter, -80f);
            audioMixer.SetFloat(buttonSoundsParameter, -80f);
        }
        else
        {
            // Restore volumes
            SetBackgroundMusicVolume(preMuteMusicVolume);
            SetButtonSoundsVolume(preMuteSoundsVolume);

            // Update sliders to reflect restored volumes
            if (backgroundMusicSlider != null)
            {
                backgroundMusicSlider.value = preMuteMusicVolume;
            }

            if (buttonSoundsSlider != null)
            {
                buttonSoundsSlider.value = preMuteSoundsVolume;
            }
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        SaveSettings();
    }
}