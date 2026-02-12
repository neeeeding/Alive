using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "BlockActionSO", menuName = "SO/GoHouse/BlockActionSO")]
    public abstract class BlockActionSO : ScriptableObject
    {
        public abstract void DoBlockAction();
    }
}