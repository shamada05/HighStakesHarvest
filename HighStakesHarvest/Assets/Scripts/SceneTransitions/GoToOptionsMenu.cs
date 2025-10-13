using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GoToOptionsMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Button component on the same GameObject
        Button button = GetComponent<Button>();

        // If this is a TextMeshPro Button, it still uses Unity's Button component
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("GoToOptionsMenu: No Button component found on this GameObject.");
        }
    }

    void OnButtonClick()
    {
        // Load the OptionsMenu scene
        SceneManager.LoadScene("OptionsMenu");
    }
}
