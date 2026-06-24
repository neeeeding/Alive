using _02Script.Battle.Buff;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "ArmorItemDataSO", menuName = "SO/Item/ArmorItemDataSO")]
    public class ArmorItemDataSO : ItemDataSO
    {
        [Space(25f)]
        [Header("ArmorItemDataSO------------------------")]
        public ItemDataSO baseData; //원본 값
        
        public BuffSO skillBuff; //스킬때 사용하는 버프
        public BuffGiveEntityType buffGiveEntity = BuffGiveEntityType.self; //버프 받는 이
        public float damage; //받아주는 데미지
        public float skillCoolTime; // 스킬 쿨타임
        
        
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