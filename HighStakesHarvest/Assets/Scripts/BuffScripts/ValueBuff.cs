using UnityEngine;

[CreateAssetMenu(fileName = "ValueBuff", menuName = "Buffs/CropBaseValueBuff")]
public class ValueBuff : ScriptableBuff
{
    public float ValueModifier = 1.1f; // 10% increase
    public override void Apply(GameObject target)
    {

        // Assuming a PlayerMovement component exists
       SeedData seedData = target.GetComponent<SeedData>();

        if (seedData != null)
        {
            //seedData.ApplyValueBuff(ValueModifier);
            Debug.Log($"{target.name} received Value Buff!");
        }
        else
        {
            Debug.Log($"{target.name} value not found");
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
