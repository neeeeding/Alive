using System.Collections.Generic;
using _02Script.Obj.Entity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.Battle.Entity
{
    public class BattleEntity : MonoBehaviour
    {
        [SerializeField] protected EntitySO entity;
        [SerializeField] public SpriteRenderer outline;
        
        protected List<BuffSO> buffs = new List<BuffSO>(); //받는 버프 (중복 가능)
        protected List<BattleEntity> canTargets = new List<BattleEntity>(); //타겟 후보
        protected List<BattleEntity> targets = new List<BattleEntity>(); //현재 타겟
        protected int maxHp;
        protected float curHp;
        protected float baseAttack; //평타
        protected float baseAttackDelay; //평타 딜레이
        protected float skillDamage;
        protected float skillAttackDelay;
        protected BuffSO startBuff; //시작할 때 버프
        protected BuffSO skillBuff; //스킬때 버프
        protected bool isGlobal;

        protected readonly int maxGlobal = 10;

        protected virtual void OnEnable()
        {
            RandomTarget();
        }

        #region Target
        protected virtual void RandomTarget()
        {
            int count = !isGlobal? 1 : Random.Range(0, maxGlobal + 1);
            for (int i = 0; i < count; i++)
            {
                int index =0;
                do
                {
                    index= Random.Range(0, canTargets.Count);
                } while (targets.Contains(canTargets[index]));
                Target(index);
            }
        }

        protected virtual void Target(int index) // 이 엔티티의 타겟을 정함
        {
            //제한이면 가장 오래된 타겟을 변경
            if ((!isGlobal && targets.Count > 0) || targets.Count >= maxGlobal)
            {
                targets.RemoveAt(0);
            }
            
            if(!targets.Contains(canTargets[index]))
                targets.Add(canTargets[index]);
        }
        #endregion

        #region Attack, Hit
        public virtual void Attack()
        {
            //공격하는 애니메이션 & 이펙트
            //딜레이 & 자동화

            int divide = targets.Count;
            foreach (BattleEntity target in targets)
            {
                target.Hit(baseAttack/divide);
            }
        }
        public virtual void UseSkill()
        {
            //스킬 사용 애니메이션 & 이펙트
            //딜레이 & 자동화
            
            int divide = targets.Count;
            if (!skillBuff.isDeBuff)
            {
                GetBuffs(skillBuff);
            }
            foreach (BattleEntity target in targets)
            {
                target.Hit(skillDamage/divide);
                if(skillBuff.isDeBuff)
                    target.GetBuffs(skillBuff);
            }
        }
        public virtual void Hit(float damage)
        {
            curHp -= damage;
            //맞는 애니메이션 & 이펙트
            if (DieCheck())
            {
                curHp = 0;
                Die();
            }
        }
        #endregion

        #region Buff
        protected virtual void BuffCalculate()
        {
            //받고 있는 버프
        }
        public virtual void GetBuffs(BuffSO buff) //버프를 얻음
        {
            buffs.Add(buff);
        }
        #endregion


        protected virtual bool DieCheck()
        {
            return curHp <= 0;
        }

        protected virtual void Die()
        {
            //죽는 애니메이션 & 이펙트
        }
    }
}