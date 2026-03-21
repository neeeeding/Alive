using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Battle.Entity;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class Monster : BattleEntity
    {
        public static Action<Monster> OnSelect;
        public static Action<Monster> OnDie;
        public static Action<EntitySO, Vector3, bool,int> OnExplanation; //설명용
        
        [Header("Monster")]
        [SerializeField] protected MonsterHpUI hpUI;
        
        
        #region Mouse
        public virtual void MouseEnter()
        {
            OnExplanation?.Invoke(entity, gameObject.transform.position,false,(int)curHp);
        }
        public virtual void MouseExit()
        {
            OnExplanation?.Invoke(null,Vector3.zero,false,0);
        }
        #endregion

        #region Set (+ Select)
        public MonsterSO GetMonsterType()
        {
            return entity as MonsterSO;
        }
        public void GetCanTargets(List<BattleEntity> target)
        {
            outline.color = new Color(0,0,0,0);
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
            startBuff = monster.eBuff;
            hpUI.UpdateHp(curHp, maxHp);
        }
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            hpUI.UpdateHp(curHp, maxHp);
            foreach (BuffSO buff in startBuff)
            {
                GetBuffs(buff);
            }
        }

        #region Entity
        protected override void Recovery()
        {
            base.Recovery();
            hpUI.UpdateHp(curHp, maxHp);
        }

        public override void Hit(float damage)
        {
            base.Hit(damage);
            hpUI.UpdateHp(curHp, maxHp);
        }
        protected override void HpDeBuff(EntityName name, float value)
        {
            base.HpDeBuff(name, value);
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