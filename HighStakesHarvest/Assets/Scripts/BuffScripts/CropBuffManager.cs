using System.Collections.Generic;
using UnityEngine;

public class CropBuffManager : MonoBehaviour
{
    private List<ScriptableBuff> activeBuffs = new List<ScriptableBuff>();

    public void AddBuff(ScriptableBuff buff)
    {
        if (buff == null) return;

        // Prevent adding the same permanent buff multiple times
        if (!activeBuffs.Contains(buff))
        {
            activeBuffs.Add(buff);
            buff.Apply(transform.parent.gameObject);
            Debug.Log($"Buff '{buff.BuffName}' added to {gameObject.name}.");
        }
        else
        {
            Debug.LogWarning($"Buff '{buff.BuffName}' is already active on {gameObject.name}.");
        }
    }

    public void RemoveBuff(ScriptableBuff buff)
    {
        if (buff == null) return;

        if (activeBuffs.Contains(buff))
        {
            buff.Remove(gameObject);
            activeBuffs.Remove(buff);
            Debug.Log($"Buff '{buff.BuffName}' removed from {gameObject.name}.");
        }
        else
        {
            Debug.LogWarning($"Buff '{buff.BuffName}' not found on {gameObject.name}.");
        }
    }

    // Optional: Save and Load Buffs (e.g., for persistent game state)
    public List<string> GetActiveBuffNames()
    {
        List<string> buffNames = new List<string>();
        foreach (var buff in activeBuffs)
        {
            buffNames.Add(buff.name); // Using the asset name for saving/loading
        }
        return buffNames;
    }

    public void LoadBuffs(List<string> buffNames)
    {
        foreach (string buffName in buffNames)
        {
            // Load the ScriptableObject by name (requires resources folder or asset database lookup)
            ScriptableBuff loadedBuff = Resources.Load<ScriptableBuff>("Buffs/" + buffName); // Assuming buffs are in a Resources/Buffs folder
            if (loadedBuff != null)
            {
                AddBuff(loadedBuff);
            }
            else
            {
                Debug.LogError($"Could not load buff with name: {buffName}");
            }
        }
    }
}
