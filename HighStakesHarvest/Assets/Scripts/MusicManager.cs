using UnityEngine;
using System.IO;

[System.Serializable]
public class AudioSettingsData
{
    public float musicVolume;
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    private string settingsPath;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // Create a save path
        settingsPath = Path.Combine(Application.persistentDataPath, "audioSettings.json");

        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            AudioSettingsData data = JsonUtility.FromJson<AudioSettingsData>(json);
            audioSource.volume = data.musicVolume;
        }
        else
        {
            // Default to mid volume
            audioSource.volume = 0.5f;
            SaveAudioSettings();
        }
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        SaveAudioSettings();
    }

    private void SaveAudioSettings()
    {
        AudioSettingsData data = new AudioSettingsData();
        data.musicVolume = audioSource.volume;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(settingsPath, json);
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }
}
