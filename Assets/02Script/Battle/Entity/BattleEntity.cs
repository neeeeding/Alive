using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Obj.Entity;
using _02Script.GamePlayer.State;
using _02Script.UI.person;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.Battle.Entity
{
    public class BattleEntity : MonoBehaviour
    {
        public static Action<List<BattleEntity>,BattleEntity> OnTarget;
        public static Action<PlayerStateType> OnAction;
        
        [SerializeField] protected BuffManager buffManager;
        [SerializeField] protected EntitySO entity;
        [SerializeField] public SpriteRenderer outline;
        
        public EntityName EntityName{get => entity.EntityName;}
        
        protected List<BattleEntity> canTargets = new List<BattleEntity>(); //타겟 후보
        protected List<BattleEntity> targets = new List<BattleEntity>(); //현재 타겟
        protected int maxHp;
        protected float curHp;
        protected float baseAttack; //평타
        protected float baseAttackDelay; //평타 딜레이
        protected float skillDamage;
        protected float skillAttackDelay;
        protected List<BuffSO> startBuff; //시작할 때 버프
        protected List<BuffSO> skillBuff; //스킬때 버프
        protected bool isGlobal;

        protected readonly int maxGlobal = 10;
        protected readonly float recoveryDelay = 1;

        protected float curAttackDelay;
        protected float curSkillDelay;
        protected float curRecoveryDelay;

        protected virtual void OnEnable()
        {
            curAttackDelay = 0;
            curSkillDelay = 0;
            Buff.Buff.OnDamage += HpDeBuff;
        }

        protected virtual void Awake()
        {
            buffManager.SetEntity(entity.EntityName);
        }

        protected virtual void OnDisable()
        {
            Buff.Buff.OnDamage -= HpDeBuff;
        }

        protected virtual void Update()
        {
            curAttackDelay += Time.deltaTime;
            curSkillDelay += Time.deltaTime;
            curRecoveryDelay += Time.deltaTime;
            
            Recovery();
            Attack();
            OnTarget?.Invoke(targets,this);
        }

        #region Target
        protected virtual void RandomTarget()
        {
            int count = !isGlobal? 1 : Random.Range(0, maxGlobal + 1);
            for (int i = 0; i < count; i++)
            {
                int index =Random.Range(0, canTargets.Count);
                if(canTargets.Count <= 0) return;
                Target(index);
            }
        }

        protected virtual void Target(int index) // 이 엔티티의 타겟을 정함
        {
            //제한이면 가장 오래된 타겟을 변경
            if ((!isGlobal && targets.Count > 0) || targets.Count >= maxGlobal)
            {
                canTargets.Add(targets[0]);
                targets.RemoveAt(0);
            }

            if (!targets.Contains(canTargets[index]))
            {
                targets.Add(canTargets[index]);
                canTargets.RemoveAt(index);
            }
        }
        #endregion

        #region Attack, Hit
        public virtual void Attack()
        {
            if(curAttackDelay < baseAttackDelay) return;
            curAttackDelay = 0;
            
            OnAction?.Invoke(PlayerStateType.Attack);

            int divide = targets.Count; //타겟 만큼 나누기
            float attack = EtcStat(StatsType.attack);
            float damage = (baseAttack + attack) / divide;
            
            foreach (BattleEntity target in targets.ToArray())
            {
                target.Hit(damage);
            }
        }
        public virtual void UseSkill()
        {
            float curDelay = skillAttackDelay - EtcStat(StatsType.skill); //쿨타임 감소
            if(curSkillDelay < curDelay || skillAttackDelay < 0) return;
            
            curSkillDelay = 0;
            curAttackDelay = 0;
            OnAction?.Invoke(PlayerStateType.Skill);
            
            int divide = targets.Count; //타겟 만큼 나누기
            float attack = ((EtcStat(StatsType.attack) -3)/2 
                            +EtcStat(StatsType.skill)/2); //((타격-3)/2 + 숙련/2)
            float damage = (skillDamage + attack) / divide;
            
            if (skillBuff!= null&&skillBuff.Count >0)
            {
                foreach (BuffSO buff in skillBuff)
                {
                    if(!buff.isDeBuff)
                        GetBuffs(buff);
                }
            }
            foreach (BattleEntity target in targets.ToArray())
            {
                target.Hit(damage);
                if(skillBuff!= null&&skillBuff.Count >0)
                    foreach (BuffSO buff in skillBuff)
                    {
                        if(buff.isDeBuff)
                            target.GetBuffs(buff);
                    }
            }
        }
        public virtual void Hit(float damage)
        {
            if(!Agility()) return;

            if(EtcStat(StatsType.defense) != 0)
                damage /= EtcStat(StatsType.defense);
            curHp -= damage;
            OnAction?.Invoke(PlayerStateType.Hit);
            if (DieCheck())
            {
                curHp = 0;
                Die();
            }
        }
        #endregion

        #region Stat
        protected virtual bool Agility() //민첩
        {
            float agilityBuff = BuffCalculate(StatsType.agility); //민첩 버프
            float randAgility = Random.Range(0, 100.1f);
            return(randAgility > agilityBuff);
        }
        protected virtual float EtcStat(StatsType type) //외 스탯들
        {
            return BuffCalculate(type); //버프
        }
        protected virtual void Recovery() //회복
        {
            if(curRecoveryDelay < recoveryDelay) return;

            curRecoveryDelay = 0;
            float recoveryBuff = BuffCalculate(StatsType.recovery);
            curHp += recoveryBuff;
        }

        protected virtual void HpDeBuff(EntityName name, float value)
        {
            if(entity.EntityName != name) return;
            
            curHp -= value;
        }
        #endregion
        
        #region Buff
        protected virtual float BuffCalculate(StatsType stat) //버프 효과 계산
        {
            return buffManager.BuffCalculate(stat);
        }
        
        public virtual void GetBuffs(BuffSO buff) //버프를 얻음
        {
            buffManager.GetBuffs(buff);
        }
        #endregion

        public virtual EntityName ReturnName()
        {
            return entity.EntityName;
        }

        protected virtual bool DieCheck()
        {
            return curHp <= 0;
        }

        protected virtual void Die()
        {
            OnAction?.Invoke(PlayerStateType.Die);
        }
    }
}