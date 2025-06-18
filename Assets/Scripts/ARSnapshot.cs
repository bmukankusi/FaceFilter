using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ARSnapshot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button captureButton;
    [SerializeField] private Image flashEffect;
    [SerializeField] private GameObject previewPanel;
    [SerializeField] private RawImage previewImage;
    [SerializeField] private float flashDuration = 0.1f;

    private void Start()
    {
        captureButton.onClick.AddListener(TakeSnapshot);
    }

    public void TakeSnapshot()
    {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        // Flash effect (visual feedback)
        if (flashEffect != null)
        {
            flashEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            flashEffect.gameObject.SetActive(false);
        }

        // Wait for rendering to complete
        yield return new WaitForEndOfFrame();

        // Capture the screen
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();

        // Show preview
        if (previewPanel != null && previewImage != null)
        {
            previewImage.texture = screenshot;
            previewPanel.SetActive(true);
        }
    }

    public void SaveScreenshot()
    {
        if (previewImage.texture != null)
        {
            Texture2D screenshot = (Texture2D)previewImage.texture;
            byte[] bytes = screenshot.EncodeToPNG();

            // Save to persistent data path (app's private storage)
            string folderPath = Application.persistentDataPath + "/Screenshots";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = "AR_Photo_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            string fullPath = Path.Combine(folderPath, fileName);
            File.WriteAllBytes(fullPath, bytes);

            Debug.Log("Saved screenshot to: " + fullPath);
        }
    }

    public void DiscardScreenshot()
    {
        if (previewPanel != null)
        {
            previewPanel.SetActive(false);
            Destroy(previewImage.texture);
        }
    }
}