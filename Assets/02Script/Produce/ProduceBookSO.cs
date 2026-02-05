using System.Collections.Generic;
using _02Script.Inventory.Item;
using UnityEditor;
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
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if(itemName == result.itemName) return;
            itemImage = result.itemImage;
            maxCount = result.maxCount;
            category = result.category;
            itemType = result.itemType;
            stats = result.stats;
            addStats = result.addStats;
            itemName = result.itemName;
            itemExplanation = result.itemExplanation;
        }
#endif
    }
    
    [System.Serializable]
    public class ItemRow
    {
        public List<ItemDataSO> items;
    }
}