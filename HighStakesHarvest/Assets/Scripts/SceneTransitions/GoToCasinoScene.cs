using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToCasinoScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("GoToCasinoScene: No Button component found on this GameObject.");
        }
    }

    void OnButtonClick()
    {
        // use SceneLoaded to check when the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load the CasinoScene
        SceneManager.LoadScene("CasinoScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only run EndTurn when we've loaded the CasinoScene
        if (scene.name == "CasinoScene")
        {
            if (TurnManager.Instance != null)
                TurnManager.Instance.EndTurn();

            else
                Debug.LogWarning("TurnManager.Instance is null when scene loaded.");
            

            // Unsubscribe so we don't call EndTurn again for future loads
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
