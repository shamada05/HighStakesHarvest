using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TurnManager turnManager;

    //public UIManager uiManager;
    //public PlayerManager playerManager;

    void Awake()
    {
        // Make sure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persists between scenes

        // Initialize or find managers if not set
        if (turnManager == null)
            turnManager = GetComponentInChildren<TurnManager>();

        //if (uiManager == null)
        //    uiManager = GetComponentInChildren<UIManager>();

        //if (playerManager == null)
        //    playerManager = GetComponentInChildren<PlayerManager>();
    }
}
