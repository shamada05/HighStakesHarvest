using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GoToMainMenu : MonoBehaviour
{
    [Header("Optional: Assign a Button (otherwise script finds it automatically)")]
    public Button mainMenuButton;

    private void Start()
    {
        // Auto-hook if no button assigned
        if (mainMenuButton == null)
            mainMenuButton = GetComponent<Button>();

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnButtonClick);
        else
            Debug.LogError("GoToMainMenu: No Button component found on this GameObject.");
    }

    private void OnButtonClick()
    {
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");

        // Delete the old save file
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("[GoToMainMenu] Old save file deleted.");
        }

        // Create a new blank save file immediately
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.data = new SaveData();  // fresh state
            SaveManager.Instance.SaveNewGame();             // write new blank save
            Debug.Log("[GoToMainMenu] Fresh save file created.");
        }
        else
        {
            Debug.LogWarning("[GoToMainMenu] SaveManager not found. Couldn't create new save.");
        }

        // Load Main Menu
        SceneManager.LoadScene("MainMenu");
    }

}
