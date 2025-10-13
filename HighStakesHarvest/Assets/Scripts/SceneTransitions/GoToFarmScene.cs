using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToFarmScene : MonoBehaviour
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
            Debug.LogError("GoToFarmScene: No Button component found on this GameObject.");
        }
    }

    void OnButtonClick()
    {
        // Load the Farm Scene
        SceneManager.LoadScene("Farm Scene");
    }
}
