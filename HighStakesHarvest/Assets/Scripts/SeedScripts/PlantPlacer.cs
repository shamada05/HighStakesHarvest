using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantPlacer : MonoBehaviour
{
    public Tilemap soilTilemap;        // assign your Tilemap in the Inspector
    public GameObject Potato;      // assign your crop prefabs here
    public GameObject Blueberry;
    public GameObject Pumpkin;

    public int selector = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selector = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selector = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selector = 3;
        }
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = soilTilemap.WorldToCell(mouseWorldPos);


            Vector3 placePosition = soilTilemap.GetCellCenterWorld(cellPosition);

            // Check if there's already a crop there (optional)
            Collider2D hit = Physics2D.OverlapPoint(placePosition);
            if (hit == null)
            {
                if (selector == 1)
                {
                    Instantiate(Potato, placePosition, Quaternion.identity);
                    Debug.Log("Crop planted at " + cellPosition);
                }
                if (selector == 2)
                {
                    Instantiate(Blueberry, placePosition, Quaternion.identity);
                    Debug.Log("Crop planted at " + cellPosition);
                }
                if (selector == 3)
                {
                    Instantiate(Pumpkin, placePosition, Quaternion.identity);
                    Debug.Log("Crop planted at " + cellPosition);
                }
            }
            else
            {
                Debug.Log("Tile already occupied!");
            }
        }
    }
}
