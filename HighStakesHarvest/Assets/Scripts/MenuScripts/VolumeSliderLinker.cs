using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSliderLinker : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeText;

    private void Start()
    {
        if (volumeSlider != null)
        {
            // Initialize to current volume from MusicManager
            float currentVolume = MusicManager.Instance != null ? MusicManager.Instance.GetVolume() : 0.5f;
            volumeSlider.value = currentVolume;
            UpdateVolumeText(currentVolume);

            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    private void OnVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(value);

        UpdateVolumeText(value);
    }

    private void UpdateVolumeText(float value)
    {
        if (volumeText != null)
            volumeText.text = Mathf.RoundToInt(value * 100).ToString();
    }
}
