using System;
using System.Collections.Generic;
using _02Script.Battle.Entity;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class Monster : BattleEntity
    {
        public static Action<Monster> OnSelect;
        public static Action<Monster> OnDie;
        [Header("Monster")]
        [SerializeField] protected MonsterHpUI hpUI;

        #region Set (+ Select)
        public MonsterSO GetMonsterType()
        {
            return entity as MonsterSO;
        }
        public void GetCanTargets(List<BattleEntity> target)
        {
            canTargets = target;
            RandomTarget();
        }
        public void SelectMonster()
        {
            OnSelect?.Invoke(this);
        }
        public virtual void SetMonster(MonsterSO monster)
        {
            entity = monster;
            skillAttackDelay = 0;
            
            isGlobal = monster.isGlobal;
            maxHp = monster.maxHp;
            curHp = maxHp;
            baseAttack = monster.baseAttack;
            baseAttackDelay = monster.baseAttackDelay;
            startBuff = monster.useBuff;
            hpUI.UpdateHp(curHp, maxHp);
        }
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            hpUI.UpdateHp(curHp, maxHp);
            outline.color = new Color(0,0,0,0);
        }

        #region Entity
        public override void Hit(float damage)
        {
            base.Hit(damage);
            hpUI.UpdateHp(curHp, maxHp);
        }
        protected override void Die()
        {
            base.Die();
            OnDie?.Invoke(this);
        }
        #endregion
    }
}