using UnityEngine;

public class TurnManager : MonoBehaviour
{

   public void EndTurn()
    {
        Plant[] allPlants = FindObjectsByType<Plant>(FindObjectsSortMode.None);
        foreach (Plant plant in allPlants)
        {
            plant.AdvanceTurn();
        }

        Debug.Log("Turn ended");
    }

    void Update()
    {
        // When Enter is pressed, advance the turn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

}
