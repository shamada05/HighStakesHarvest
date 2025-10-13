//using UnityEngine;

//[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
//public class BuffData : ScriptableObject
//{

//}

using UnityEngine;

public abstract class ScriptableBuff : ScriptableObject
{
    public string BuffName;
    public Sprite Icon; 

    public abstract void Apply(GameObject target);
    public abstract void Remove(GameObject target);
}

