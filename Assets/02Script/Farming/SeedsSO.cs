using _02Script.Item;
using UnityEngine;

namespace _02Script.Farming
{
    [CreateAssetMenu(fileName = "SeedsSO", menuName = "SO/Farming/SeedsSO")]
    public class SeedsSO : ScriptableObject
    {
        public ItemSO seeds;
        public ItemSO fruit;
        public float growDelay = 60 * 5;
    }
}