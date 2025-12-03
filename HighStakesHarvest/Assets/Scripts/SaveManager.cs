using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class SaveData
{
    public int farmSceneTutoSeen = 0;
    public int casinoSceneTutoSeen = 0;
    public int slotsSceneTutoSeen = 0;
    public int casinoTableTutoSeen = 0;
    public int savedQuotaIndex = 0;
    public int savedTurnsRemaining = 0;
    public int savedMoney = 0;
    public int savedStartingMoneyForQuota = 0;
    public string inventoryJson = "";
    public List<SavedPlant> savedPlants = new List<SavedPlant>();
}

[System.Serializable]
public class SavedPlant
{
    public string cropName;

    public float posX;
    public float posY;

    public int currentStage;
    public int turnsGrown;
    public bool needsWater;
    public int timesHarvested;
    public bool isOnTilledSoil;
}


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("Tutorial Prefabs (assign in Inspector)")]
    public GameObject farmSceneTutorialPrefab;
    public GameObject casinoSceneTutorialPrefab;
    public GameObject slotsTutorialPrefab;
    public GameObject casinoTableTutorialPrefab;

    private string savePath;
    public SaveData data;

    private bool plantsLoadedThisSession = false;

    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        // Load if file exists (do not create automatically here —
        // creation will be triggered by the Play button as you requested)
        if (File.Exists(savePath))
            LoadGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called by Play button before first scene load
    public void InitializeSaveFile()
    {
        if (File.Exists(savePath))
        {
            LoadGame();
        }
        else
        {
            data = new SaveData();
            SaveGame();
            Debug.Log("[SaveManager] New save file created at: " + savePath);
        }
    }

    private void LoadGame()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                data = JsonUtility.FromJson<SaveData>(json);

                if (data == null)
                    data = new SaveData();

                Debug.Log("[SaveManager] Loaded save file.");
            }
            else
            {
                data = new SaveData();
                SaveGame(); // Create a save file immediately
                Debug.Log("[SaveManager] No save file found. Created new one.");
            }
        }
        catch
        {
            data = new SaveData();
            Debug.LogWarning("[SaveManager] Failed to load save file, using defaults.");
        }

        // --- Load Inventory ------------------------
        if (PlayerInventory.Instance != null)
        {
            if (!string.IsNullOrEmpty(data.inventoryJson))
            {
                PlayerInventory.Instance.LoadInventoryData(data.inventoryJson);
                Debug.Log("[SaveManager] Loaded player inventory.");
            }
            else
            {
                Debug.Log("[SaveManager] No inventory found in save. Starting empty.");
            }
        }
        else
        {
            Debug.LogWarning("[SaveManager] PlayerInventory not found in scene during load.");
        }

        if (QuotaManager.Instance != null)
        {
            QuotaManager.Instance.RestoreQuotaState(
                data.savedQuotaIndex,
                data.savedTurnsRemaining,
                data.savedStartingMoneyForQuota
            );

            Debug.Log("[SaveManager] Quota restored: index=" + data.savedQuotaIndex +
                      " turns=" + data.savedTurnsRemaining +
                      " startMoney=" + data.savedStartingMoneyForQuota);
        }

        // --- Load Money ---
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.SetMoney(data.savedMoney);
            Debug.Log("[SaveManager] Money restored: " + data.savedMoney);
        }
        else
        {
            Debug.LogWarning("[SaveManager] MoneyManager not loaded yet.");
        }


    }


    public void SaveGame()
    {
        try
        {
            // --- Save Inventory ------------------------
            if (PlayerInventory.Instance != null)
            {
                data.inventoryJson = PlayerInventory.Instance.SaveInventoryData();
                Debug.Log("[SaveManager] Saved player inventory.");
            }
            else
            {
                Debug.LogWarning("[SaveManager] PlayerInventory not found during save.");
            }

            SavePlants();

            // --- Write Save File ------------------------
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);

            Debug.Log("[SaveManager] Save file written.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[SaveManager] Failed to write save file: " + ex.Message);
        }
    }


    // Scene loaded -> try show tutorial (single place of truth)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[SaveManager] Scene loaded: " + scene.name);

        switch (scene.name)
        {
            case "FarmScene":
                TryShowTutorial(ref data.farmSceneTutoSeen, farmSceneTutorialPrefab, "FarmScene");
                if (!plantsLoadedThisSession)
                {
                    StartCoroutine(LoadPlantsDelayed());
                }
                break;
            case "CasinoScene":
                TryShowTutorial(ref data.casinoSceneTutoSeen, casinoSceneTutorialPrefab, "CasinoScene");
                break;
            case "Slots":
                TryShowTutorial(ref data.slotsSceneTutoSeen, slotsTutorialPrefab, "Slots");
                break;
            case "CasinoTable":
                TryShowTutorial(ref data.casinoTableTutoSeen, casinoTableTutorialPrefab, "CasinoTable");
                break;
            default:
                // nothing to do
                break;
        }
    }

    private void TryShowTutorial(ref int flag, GameObject prefab, string sceneLabel)
    {
        if (flag == 1)
        {
            Debug.Log($"[SaveManager] {sceneLabel} tutorial already seen (flag=1).");
            return;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"[SaveManager] Tutorial prefab for {sceneLabel} is not assigned.");
            return;
        }

        Canvas targetCanvas = FindObjectOfType<Canvas>();
        GameObject instance;

        if (targetCanvas == null)
        {
            Debug.LogWarning($"[SaveManager] No Canvas found in scene {sceneLabel}. Instantiating at root.");
            instance = Instantiate(prefab);
        }
        else
        {
            instance = Instantiate(prefab, targetCanvas.transform);

            // Keep prefab’s own RectTransform settings!
            RectTransform rt = instance.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition3D = Vector3.zero;
            }
        }

        Debug.Log($"[SaveManager] Spawned tutorial prefab for {sceneLabel}.");

        flag = 1;
        SaveGame();
    }

    public void ContinueGame()
    {
        SaveManager.Instance.LoadGame();

        int quotaIndex = SaveManager.Instance.data.savedQuotaIndex;
        int turns = SaveManager.Instance.data.savedTurnsRemaining;
        int money = SaveManager.Instance.data.savedMoney;

        // Load the actual play scene
        SceneManager.LoadScene("FarmScene");  // or whatever first gameplay scene

        // After scene loads:
        StartCoroutine(LoadRunDelayed(quotaIndex, turns, money));
    }

    private IEnumerator LoadRunDelayed(int quotaIndex, int turns, int money)
    {
        // --- Wait until MoneyManager exists ---
        while (MoneyManager.Instance == null)
            yield return null;

        // Apply saved money after MoneyManager initialized
        MoneyManager.Instance.SetMoney(money);

        // --- Wait until QuotaManager exists ---
        while (QuotaManager.Instance == null)
            yield return null;

        // Now safely restore quota data
        QuotaManager.Instance.RestoreQuotaState(
            quotaIndex,
            turns,
            SaveManager.Instance.data.savedStartingMoneyForQuota
        );

        Debug.Log("[MainMenu] Loaded saved quota/turn progress.");
    }


    public int GetStartingMoney()
    {
        return data.savedStartingMoneyForQuota;
    }

    public void SavePlants()
    {
        data.savedPlants.Clear();

        if (PlantManager.Instance == null)
            return;

        foreach (GameObject p in PlantManager.Instance.Plants)
        {
            if (p == null) continue;

            PlantGrowth pg = p.GetComponent<PlantGrowth>();
            if (pg == null) continue;

            SavedPlant sp = new SavedPlant();
            sp.cropName = pg.seedData.cropName;
            sp.posX = p.transform.position.x;
            sp.posY = p.transform.position.y;
            sp.currentStage = pg.currentStage;
            sp.turnsGrown = pg.turnsGrown;
            sp.needsWater = pg.needsWater;
            sp.timesHarvested = pg.timesHarvested;
            sp.isOnTilledSoil = pg.isOnTilledSoil;


            data.savedPlants.Add(sp);
        }

        Debug.Log($"[SaveManager] Saved {data.savedPlants.Count} plants.");
    }

    private IEnumerator LoadPlantsDelayed()
    {
        // Wait until PlantManager + PlantPlacer exist
        while (PlantManager.Instance == null)
            yield return null;

        PlantPlacer placer = null;
        while (placer == null)
        {
            placer = FindObjectOfType<PlantPlacer>();
            yield return null;
        }

        // Now restore plants
        foreach (var sp in data.savedPlants)
        {
            if (!placer.seedDict.TryGetValue(sp.cropName, out GameObject prefab))
            {
                Debug.LogWarning($"No prefab found for saved crop '{sp.cropName}'");
                continue;
            }

            Vector3 pos = new(sp.posX, sp.posY);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            PlantGrowth pg = go.GetComponent<PlantGrowth>();
            pg.currentStage = sp.currentStage;
            pg.turnsGrown = sp.turnsGrown;
            pg.needsWater = sp.needsWater;
            pg.timesHarvested = sp.timesHarvested;
            pg.isOnTilledSoil = sp.isOnTilledSoil;


            PlantManager.Instance.AddPlant(go);
        }

        plantsLoadedThisSession = true;

        Debug.Log($"[SaveManager] Loaded {data.savedPlants.Count} plants.");
    }

}
