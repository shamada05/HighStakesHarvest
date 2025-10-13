// Example: Speed Buff
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedBuff", menuName = "Buffs/Speed Buff")]
public class SpeedBuff : ScriptableBuff
{
    public float SpeedModifier = 1.2f; // 20% speed increase

    public override void Apply(GameObject target)
    {

        // Assuming a PlayerMovement component exists
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();


        if (playerMovement != null)
        {
            playerMovement.ApplySpeedBuff(SpeedModifier);
            Debug.Log($"{target.name} received Speed Buff!");
        }
        else
        {
            Debug.Log($"{target.name} Player movement not found");
        }
    }

    public override void Remove(GameObject target)
    {
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.RemoveSpeedBuff(SpeedModifier);
            Debug.Log($"{target.name} Speed Buff removed.");
        }
    }
}
