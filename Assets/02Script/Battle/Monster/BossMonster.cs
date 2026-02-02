using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class BossMonster : Monster
    {
        [Header("BossMonster")]
        [SerializeField] private BossMonsterHpUI hpUI;
        protected override void OnEnable()
        {
            hpUI.SetCharacter(entity.EntityName);
        }

        public void SetMonster(BossMonsterSO monster)
        {
            base.SetMonster(monster);
            
            skillBuff = monster.useSkillBuff;
            skillDamage = monster.skillAttack;
            skillAttackDelay = monster.skillAttackDelay;
        }
    }
}