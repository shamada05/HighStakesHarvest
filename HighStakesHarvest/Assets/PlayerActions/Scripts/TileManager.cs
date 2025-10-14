using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{

    [SerializeField] private Tilemap interactableMap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sets tiles to invisible
        interactableMap.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
