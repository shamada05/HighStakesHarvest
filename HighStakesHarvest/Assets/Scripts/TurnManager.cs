using UnityEngine;
using UnityEngine.SceneManagement; 

public class TurnManager : MonoBehaviour
{

    public static TurnManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void EndTurn()
    {
        // Iterate through plants in plant manager and advance turn
        if (PlantManager.Instance != null)
        {
            var plants = PlantManager.Instance.Plants;
            foreach (var go in plants)
            {
                if (go == null) continue;
                Plant plantComp = go.GetComponent<Plant>();
                if (plantComp != null)
                    plantComp.AdvanceTurn();
            }
        }
        /*
        else
        {
            // Fallback: find all Plant instances in the scene
            Plant[] allPlants = FindObjectsByType<Plant>(FindObjectsSortMode.None);
            foreach (Plant plant in allPlants)
            {
                plant.AdvanceTurn();
            }
        }
        */

        Debug.Log("Turn ended");
    }

    /*
    void Update()
    {
        // When Enter is pressed, advance the turn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }
    */
}
