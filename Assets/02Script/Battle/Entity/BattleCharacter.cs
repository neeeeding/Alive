using System;
using System.Collections.Generic;
using _02Script.Battle.UI;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Battle.Entity
{
    public class BattleCharacter : BattleEntity
    {
        public static Action OnDie;
        public static Action<WeaponItemDataSO> OnWeapon;
        
        [Header("Player")]
        [SerializeField] private CharacterHpUI hpUI;
        [SerializeField] private CharacterHpUI inGameHpUI;
        [SerializeField] private SkillBtn skillDelayUI;
        [SerializeField] private Color outlineColor;
        
        private static BattleCharacter _selectPlayer; //선택 중인 플레이어
        private readonly float _baseAttackCoolTime = 2f; //평타 딜레이
        
        private WeaponItemDataSO _useWeapon; //사용 중인 무기
        private Dictionary<StatsType, float> _stats = new Dictionary<StatsType, float>(); //스탯들

        private bool _isSetStat;

        #region EnDiSt
        protected override void OnEnable()
        {
            _isSetStat = false;
            base.OnEnable();
            Monster.Monster.OnSelect += Target;
            GameManager.OnStart += SetStats;
            Monster.Monster.OnDie += TargetDie;
            hpUI.SetCharacter(entity.EntityName);
            inGameHpUI.SetCharacter(entity.EntityName);
            hpUI.UpdateHp(curHp, maxHp);
            inGameHpUI.UpdateHp(curHp, maxHp);
            
            RandomTarget();
        }

        private void Start()
        {
            outline.color = outlineColor;
        }

        private void OnDisable()
        {
            Monster.Monster.OnSelect -= Target;
            GameManager.OnStart -= SetStats;
            Monster.Monster.OnDie -= TargetDie;
        }

        #endregion

        #region Set
        public void GetCanTargets(BattleEntity target)
        {
            canTargets.Add(target);
            if (targets.Count <= 0)
            {
                RandomTarget();
            }
        }
        private void ChangeWeapon(WeaponItemDataSO weapon)
        {
            _useWeapon = weapon;
            isGlobal = weapon.isGlobal;
            skillBuff = weapon.skillBuff;

            float skillDamageAdd = (_stats[StatsType.skill] / 2) + ((_stats[StatsType.attack] -3)/ 2);
            //스킬 대미지는 타격/2 + 숙련/2 정도를 추가로 더 받는다.
            skillDamage = weapon.skillDamage
            + (skillDamage / 100) * skillDamageAdd;
            skillAttackDelay = weapon.skillCoolTime 
                - (skillAttackDelay / 100) * (100 - _stats[StatsType.skill]);
            skillAttackDelay = Mathf.Max(0, skillAttackDelay);
            
            OnWeapon?.Invoke(weapon);
        }
        public void SetCharacter(WeaponItemDataSO weapon, CharacterHpUI hp = null,EntitySO character = null)
        {
            if (_stats.Count <= 0) SetStats();
            
            if(character!= null)
                entity = character;
            //hpUI = hp;
            
            maxHp = (int)_stats[StatsType.HpStat];
            curHp = maxHp;
            baseAttack = _stats[StatsType.attack];
            baseAttackDelay = _baseAttackCoolTime
                - (_baseAttackCoolTime / 100) * _stats[StatsType.skill];
            baseAttackDelay = Mathf.Max(0, baseAttackDelay);
            
            ChangeWeapon(weapon);
            hpUI.SetCharacter(entity.EntityName);
            inGameHpUI.SetCharacter(entity.EntityName);
            hpUI.UpdateHp(curHp, maxHp);
            inGameHpUI.UpdateHp(curHp, maxHp);
        }

        private void SetStats()
        {
            if(_isSetStat) return;
            Dictionary<StatsType, int> stats = GameManager.Instance.PlayerStat.characterStats[entity.EntityName].ToDictionary();
            _stats.Clear();
            foreach (StatsType sta in Enum.GetValues(typeof(StatsType)))
            {
                _stats.Add(sta,StatCalculate.Calculate(entity.EntityName, sta));
            }
            _isSetStat = true;
        }
        #endregion

        #region Target(+Select)
        public void SelectPlayer()
        {
            if (_selectPlayer == this) //선택 취소
            {
                _selectPlayer = null;
                return;
            }
            _selectPlayer = this;
        }

        private void TargetDie(Monster.Monster monster)
        {
            targets.Remove(monster);
            canTargets.Remove(monster); 
            RandomTarget();
        }

        protected override void Target(int index)
        {
            _selectPlayer = this;
            Target(canTargets[index]);
            _selectPlayer = null;
        }

        private void Target(BattleEntity target)
        {
            if(_selectPlayer != this) return;
            if (targets.Contains(target))
            {
                if (isGlobal) //다중이면 선택 취소 가능
                {
                    target.outline.color = new Color(0,0,0,0);
                    canTargets.Add(target);
                    targets.Remove(target);
                }
                return;
            }
            
            if ((!isGlobal && targets.Count > 0) || targets.Count >= maxGlobal) //가능한 타겟수 찼다면 맨 처음을 변경
            {
                targets[0].outline.color = new Color(0,0,0,0);
                canTargets.Add(targets[0]);
                targets.RemoveAt(0);
            }

            targets.Add(target);
            canTargets.Remove(target);
            target.outline.color += outlineColor;
        }
        #endregion

        #region Entity
        protected override void Update()
        {
            base.Update();
            skillDelayUI.SkillDelay(curSkillDelay,skillAttackDelay);
        }

        public override void Hit(float damage)
        {
            base.Hit(damage);
            hpUI.UpdateHp(curHp, maxHp);
            inGameHpUI.UpdateHp(curHp, maxHp);
        }
        protected override void Die()
        {
            base.Die();
            OnDie?.Invoke();
            Time.timeScale = 0;
        }
        #endregion
    }
}