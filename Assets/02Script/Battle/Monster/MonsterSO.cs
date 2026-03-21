using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Obj.Entity;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Battle.Monster
{
    [CreateAssetMenu(fileName = "MonsterSO", menuName = "SO/Entity/Monster", order = 0)]
    public class MonsterSO : EntitySO
    {
        [Space(25f)]
        [Header("MonsterSO------------------------")]
        public int maxHp;
        public float baseAttack; //평타
        public float baseAttackDelay; //평타 딜레이
        [FormerlySerializedAs("eBuff")] public List<BuffSO> useBuff; //사용하는 버프
        public bool isGlobal; //전역인지 단일인지
    }
}