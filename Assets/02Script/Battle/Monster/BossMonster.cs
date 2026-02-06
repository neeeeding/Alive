namespace _02Script.Battle.Monster
{
    public class BossMonster : Monster
    {
        private  BossMonsterHpUI _bossHpUI;
        protected override void OnEnable()
        {
            base.OnEnable();
            if(_bossHpUI != null)
                _bossHpUI.SetCharacter(entity.EntityName);
        }

        protected override void Update()
        {
            base.Update();
            UseSkill();
        }

        public void SetMonster(BossMonsterSO monster, BossMonsterHpUI hp)
        {
            base.SetMonster(monster);
            //_bossHpUI = hp;
            _bossHpUI = hpUI as BossMonsterHpUI;
            hpUI = hp;
            
            skillBuff = monster.useSkillBuff;
            skillDamage = monster.skillAttack;
            skillAttackDelay = monster.skillAttackDelay;
            _bossHpUI.UpdateHp(curHp, maxHp);
            _bossHpUI.SetCharacter(entity.EntityName);
        }
    }
}