using _02Script.Item;
using UnityEngine;

namespace _02Script.Farming
{
    [CreateAssetMenu(fileName = "SeedsSO", menuName = "SO/Farming/SeedsSO")]
    public class SeedsSO : ScriptableObject
    {
        public ItemDataSO seeds;
        public ItemDataSO fruit;
        public float growDelay = 60 * 5;
    }
}