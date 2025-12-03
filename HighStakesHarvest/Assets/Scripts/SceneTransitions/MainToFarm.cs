using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainToFarm : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnButtonClick);
        else
            Debug.LogError("No Button component found on this GameObject.");
    }

    void OnButtonClick()
    {
        // Initialize save file before starting the game
        SaveManager.Instance.InitializeSaveFile();

        // If the player has already seen the opening cutscene, go straight to the farm
        if (SaveManager.Instance.data.hasSeenStartingCutscene)
        {
            SceneManager.LoadScene("FarmScene");
        }
        else
        {
            SceneManager.LoadScene("Cutscene");
        }
    }
}