using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce
{
    [CreateAssetMenu(fileName = "MeansDataSO", menuName = "SO/Item/MeansDataSO")]
    public class MeansDataSO : ItemDataSO
    {
        [Space(50)]
        [Header("Means---------------------------------------------")]
        public Sprite background;
    }
}