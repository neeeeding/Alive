using System.Collections.Generic;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce
{
    [CreateAssetMenu(fileName = "ProduceBookSO", menuName = "SO/Item/ProduceBookSO")]
    public class ProduceBookSO : ItemDataSO
    {
        [Space(50)]
        [Header("Book---------------------------------------------")]
        public List<ItemRow> itemRows;
        public MeansDataSO means; //도구
        public ItemDataSO result;
    }
    
    [System.Serializable]
    public class ItemRow
    {
        public List<ItemDataSO> items;
    }
}