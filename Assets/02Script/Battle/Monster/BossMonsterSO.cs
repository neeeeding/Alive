using System.Collections.Generic;
using _02Script.Battle.Buff;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    [CreateAssetMenu(fileName = "BossMonsterSO", menuName = "SO/Entity/BossMonster", order = 0)]
    public class BossMonsterSO : MonsterSO
    {
        [Space(25f)]
        [Header("BossMonsterSO------------------------")]
        public float skillAttack; //평타
        public float skillAttackDelay; //평타 딜레이
        public List<BuffSO> eSkillBuff; //사용하는 버프
    }
}