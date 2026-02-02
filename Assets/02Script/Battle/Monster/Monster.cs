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
        [SerializeField] private MonsterHpUI hpUI;

        #region Set (+ Select)
        public void GetCanTargets(List<BattleEntity> target)
        {
            canTargets = target;
        }
        public void SelectMonster()
        {
            OnSelect?.Invoke(this);
        }
        public virtual void SetMonster(MonsterSO monster)
        {
            entity = monster;
            
            isGlobal = monster.isGlobal;
            maxHp = monster.maxHp;
            curHp = maxHp;
            baseAttack = monster.baseAttack;
            baseAttackDelay = monster.baseAttackDelay;
            startBuff = monster.useBuff;
        }
        #endregion

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