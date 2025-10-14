using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed", menuName = "Farming/Seed Data")]
public class SeedData : ScriptableObject
{
    public string seedName;
    public int price;
    public int growthTime; // amount of turns til growth
    public string seasonPreference;
    public string type;
    public GameObject[] growthStages; // prefabs for each stage

    public void ApplyValueBuff(float modifier)
    {
        price = Mathf.CeilToInt(price*modifier);
    }

    // lowers growth time by turnDecrease
    public void ApplyGrowthBuff(int turnDecrease)
    {
        growthTime = Mathf.Max(growthTime - turnDecrease, 1); // minimum growth time is one turn (no planting crops that instantly grow...for now?)
    }


}