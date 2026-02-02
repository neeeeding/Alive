using _02Script.Battle;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "WeaponItemDataSO", menuName = "SO/Item/WeaponItemDataSO")]
    public class WeaponItemDataSO : ItemDataSO
    {
        [Space(25f)]
        [Header("WeaponItemDataSO------------------------")]
        public bool isGlobal; //전역인지 단일인지
        public BuffSO skillBuff; //스킬때 사용하는 버프
        public float skillDamage; //스킬 시 데미지
        public float skillCoolTime; // 스킬 쿨타임
    }
}