using UnityEngine;

[CreateAssetMenu(fileName = "ValueBuff", menuName = "Buffs/CropBaseValueBuff")]
public class ValueBuff : ScriptableBuff
{
    public float ValueModifier = 1.1f; // 10% increase
    public string seedToBuff = "Potato";
    public override void Apply(GameObject target) // target is actually plants manager
    {

        foreach (Transform child in target.transform)
        {
            
            Plant plant = child.GetComponent<Plant>();

            if (plant != null)
            {
                SeedData seedData = plant.seedData;
                if (seedData.name != seedToBuff) { 
                    Debug.Log($"{seedData.name} currently does not match seedToBuff");
                    continue; 
                }

                seedData.ApplyValueBuff(ValueModifier);
                Debug.Log($"{seedData.name} received Value Buff!");
            }
            else
            {
                Debug.Log($"{target.name} value not found");
            }

        }
    }

    public override void Remove(GameObject target)
    {
        return;
    }
    //{
    //    PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
    //    if (playerMovement != null)
    //    {
    //        playerMovement.RemoveSpeedBuff(SpeedModifier);
    //        Debug.Log($"{target.name} Speed Buff removed.");
    //    }
    //}

}
