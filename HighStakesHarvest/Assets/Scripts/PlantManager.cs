using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance { get; private set; }

    // The scene name in which plants should be visible
    public string visibleSceneName = "FarmScene";

    private readonly List<GameObject> plants = new List<GameObject>();

    public IReadOnlyList<GameObject> Plants => plants.AsReadOnly();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddPlant(GameObject plant)
    {
        if (plant == null) return;

        // parent under the manager so plants persist with the manager
        plant.transform.SetParent(transform);

        if (!plants.Contains(plant))
            plants.Add(plant);

        // Ensure visibility matches current active scene
        bool show = SceneManager.GetActiveScene().name == visibleSceneName;
        plant.SetActive(show);
    }

    public void RemovePlant(GameObject plant)
    {
        if (plant == null) return;
        plants.Remove(plant);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool show = scene.name == visibleSceneName;
        for (int i = 0; i < plants.Count; i++)
        {
            var p = plants[i];
            if (p != null)
                p.SetActive(show);
        }
    }
}
