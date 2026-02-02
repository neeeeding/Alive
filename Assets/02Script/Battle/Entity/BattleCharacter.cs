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
        
        [Header("Player")]
        [SerializeField] private CharacterHpUI hpUI;
        [SerializeField] private Color outlineColor;
        
        private static BattleCharacter _selectPlayer; //선택 중인 플레이어
        private readonly float _baseAttackCoolTime = 2f; //평타 딜레이
        
        private WeaponItemDataSO _useWeapon; //사용 중인 무기
        private Dictionary<StatsType, float> _stats = new Dictionary<StatsType, float>(); //스탯들

        #region EnDiSt
        protected override void OnEnable()
        {
            base.OnEnable();
            Monster.Monster.OnSelect += Target;
            GameManager.OnStart += SetStats;
            hpUI.SetCharacter(entity.EntityName);
        }

        private void Start()
        {
            outline.color = outlineColor;
        }

        private void OnDisable()
        {
            Monster.Monster.OnSelect -= Target;
            GameManager.OnStart -= SetStats;
        }

        #endregion

        #region Set
        private void GetCanTargets(BattleEntity target)
        {
            canTargets.Add(target);
        }
        private void ChangeWeapon(WeaponItemDataSO weapon)
        {
            _useWeapon = weapon;
            isGlobal = weapon.isGlobal;
            skillBuff = weapon.skillBuff;
            skillDamage = weapon.skillDamage;
            skillAttackDelay = weapon.skillCoolTime;
        }
        public void SetCharacter(EntitySO character, WeaponItemDataSO weapon)
        {
            entity = character;
            
            maxHp = (int)_stats[StatsType.HpStat];
            curHp = maxHp;
            baseAttack = _stats[StatsType.attack];
            baseAttackDelay = _baseAttackCoolTime
                - (_baseAttackCoolTime / 100) * (100 - _stats[StatsType.skill]);
            
            ChangeWeapon(weapon);
        }

        private void SetStats()
        {
            Dictionary<StatsType, int> stats = GameManager.Instance.PlayerStat.characterStats[entity.EntityName].ToDictionary();
            _stats.Clear();
            foreach (StatsType sta in Enum.GetValues(typeof(StatsType)))
            {
                _stats.Add(sta,StatCalculate.Calculate(entity.EntityName, sta));
            }
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

        protected override void Target(int index)
        {
            base.Target(index);
            canTargets[index].outline.color = outlineColor;
        }

        private void Target(BattleEntity target)
        {
            if(_selectPlayer != this) return;
            if (targets.Contains(target))
            {
                if (isGlobal)
                {
                    target.outline.color = new Color(0,0,0,0);
                    targets.Remove(target);
                }
                return;
            }
            
            if ((!isGlobal && targets.Count > 0) || targets.Count >= maxGlobal)
            {
                targets[0].outline.color = new Color(0,0,0,0);
                targets.RemoveAt(0);
            }

            targets.Add(target);
            target.outline.color = outlineColor;
        }
        #endregion
        
        protected override void Die()
        {
            base.Die();
            OnDie?.Invoke();
        }
    }
}