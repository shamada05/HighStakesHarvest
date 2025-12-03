using System.Collections.Generic;
using UnityEngine;

public class BuffDatabase : MonoBehaviour
{
    public static BuffDatabase Instance;

    public List<ScriptableBuff> allBuffs = new List<ScriptableBuff>();
    private Dictionary<string, ScriptableBuff> lookup = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        lookup.Clear();
        foreach (var buff in allBuffs)
        {
            if (buff == null) continue;
            string id = !string.IsNullOrEmpty(buff.BuffID) ? buff.BuffID : buff.BuffName;
            lookup[id] = buff;
        }
    }

    public ScriptableBuff GetBuffById(string id)
    {
        lookup.TryGetValue(id, out var buff);
        return buff;
    }
}
