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
}