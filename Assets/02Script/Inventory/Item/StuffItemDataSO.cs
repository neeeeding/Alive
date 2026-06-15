using _02Script.Battle.Buff;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "StuffItemDataSO", menuName = "SO/Item/StuffItemDataSO")]
    public class StuffItemDataSO : ItemDataSO
    {
        [Space(25f)] [Header("StuffItemDataSO------------------------")]
        public ItemDataSO baseData; //원본 값

        public BuffSO[] buffs; //버프
        public WeaponItemDataSO weapon;
        public ArmorItemDataSO armor;


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if(itemImage == baseData.itemImage) return;
            itemImage = baseData.itemImage;
            maxCount = baseData.maxCount;
            category = baseData.category;
            itemType = baseData.itemType;
            stats = baseData.stats;
            addStats = baseData.addStats;
            itemName = baseData.itemName;
            itemExplanation = baseData.itemExplanation;
            base.OnValidate();
        }
#endif
    }
}
