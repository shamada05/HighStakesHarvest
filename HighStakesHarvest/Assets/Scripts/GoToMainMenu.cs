using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToMainMenu : MonoBehaviour
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
            Debug.LogError("GoToMainMenu: No Button component found on this GameObject.");
        }
    }

    void OnButtonClick()
    {
        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
