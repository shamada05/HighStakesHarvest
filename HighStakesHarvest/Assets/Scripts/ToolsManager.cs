using UnityEngine;

public enum PlayerAction
{
    None,
    Plant1,
    Plant2,
    Plant3,
    Water,
    Harvest
}

public class ToolsManager : MonoBehaviour
{
    public static ToolsManager Instance;

    public PlayerAction currentAction = PlayerAction.None;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void SetAction_Water()
    {
        currentAction = PlayerAction.Water;
        Debug.Log("Switched to Watering mode");
    }

    public void SetAction_Harvest()
    {
        currentAction = PlayerAction.Harvest;
        Debug.Log("Switched to Harvesting mode");
    }

    public void SetAction_Plant1()
    {
        currentAction = PlayerAction.Plant1;
        Debug.Log("Switched to Potato Planting mode");
    }
    public void SetAction_Plant2()
    {
        currentAction = PlayerAction.Plant2;
        Debug.Log("Switched to Blueberry Planting mode");
    }
    public void SetAction_Plant3()
    {
        currentAction = PlayerAction.Plant3;
        Debug.Log("Switched to Pumpkin Planting mode");
    }

    public void ClearAction()
    {
        currentAction = PlayerAction.None;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAction_Plant1 ();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAction_Plant2 ();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAction_Plant3 ();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetAction_Water ();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetAction_Harvest ();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearAction();
        }
    }
}
