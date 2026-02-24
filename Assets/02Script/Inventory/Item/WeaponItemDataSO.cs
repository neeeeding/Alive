using _02Script.Battle.Buff;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "WeaponItemDataSO", menuName = "SO/Item/WeaponItemDataSO")]
    public class WeaponItemDataSO : ItemDataSO
    {
        [Space(25f)]
        [Header("WeaponItemDataSO------------------------")]
        public ItemDataSO baseData; //원본 값
        
        public bool isGlobal; //전역인지 단일인지
        public BuffSO skillBuff; //스킬때 사용하는 버프
        public BuffGiveEntityType buffGiveEntity = BuffGiveEntityType.self; //버프 받는 이
        public float skillDamage; //스킬 시 데미지
        public float skillCoolTime; // 스킬 쿨타임
        
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if(itemName == baseData.itemName) return;
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

    public enum BuffGiveEntityType
    {
        none = 0,
        self = 1,
        enemy = 2,
        otherPlayer = 3, //아군
    }
}