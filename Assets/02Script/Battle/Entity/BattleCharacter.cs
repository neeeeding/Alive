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
        public static Action<WeaponItemDataSO> OnChangeWeapon;
        public static Action<WeaponInventoryCard,float> OnSkillWeapon;
        
        [Header("Player")]
        [SerializeField] private CharacterHpUI hpUI;
        [SerializeField] private Color outlineColor;
        
        private static BattleCharacter _selectPlayer; //선택 중인 플레이어
        private readonly float _baseAttackCoolTime = 2f; //평타 딜레이
        private readonly float _baseWeaponDemage = 5f; //무기 내구도 닳는 정도

        private ForCharacterUI _forCharacter; //기타 ui
        private WeaponInventoryCard _useWeapon; //사용 중인 무기
        private Dictionary<StatsType, float> _stats = new Dictionary<StatsType, float>(); //스탯들

        private bool _isSetStat;

        #region EnDiSt
        protected override void OnEnable()
        {
            _isSetStat = false;
            base.OnEnable();
            Monster.Monster.OnSelect += Target;
            HouseManager.OnStart += SetStats;
            Monster.Monster.OnDie += TargetDie;
            WeaponInventoryCard.OnMouseClick += ChangeWeapon;
            hpUI.SetCharacter(entity.EntityName);
            hpUI.UpdateHp(curHp, maxHp);
            if(_forCharacter != null)
                _forCharacter.SetHpUI(curHp, maxHp,entity.EntityName);
            
            RandomTarget();
        }

        private void Start()
        {
            outline.color = outlineColor;
        }

        private void OnDisable()
        {
            Monster.Monster.OnSelect -= Target;
            HouseManager.OnStart -= SetStats;
            Monster.Monster.OnDie -= TargetDie;
            WeaponInventoryCard.OnMouseClick -= ChangeWeapon;
        }

        #endregion

        public BattleEntitySO ReturnSO()
        {
            return entity as BattleEntitySO;
        }

        #region Set
        public void GetCanTargets(BattleEntity target)
        {
            canTargets.Add(target);
            if (targets.Count <= 0)
            {
                RandomTarget();
            }
        }
        public void ChangeWeapon(WeaponInventoryCard weapon,EntityName entityName)
        {
            if (!weapon|| entityName != entity.EntityName)
            {
                return;
            }

            curSkillDelay = 0;
            
            if (_stats.Count <= 0) SetStats();
            
            _useWeapon = weapon;
            WeaponItemDataSO so = weapon.ReturnData().ReturnDataSO() as WeaponItemDataSO;
            isGlobal = so.isGlobal;
            skillBuff = so.skillBuff;

            float skillDamageAdd = (_stats[StatsType.skill] / 2) + ((_stats[StatsType.attack] -3)/ 2);
            //스킬 대미지는 타격/2 + 숙련/2 정도를 추가로 더 받는다.
            skillDamage = so.skillDamage
                          + (skillDamage / 100) * skillDamageAdd;
            skillAttackDelay = so.skillCoolTime 
                               - (so.skillCoolTime / 100) * (_stats[StatsType.skill]);
            
            skillAttackDelay = Mathf.Max(0, skillAttackDelay);
            
            if(_forCharacter)
                _forCharacter.ChangeWeapon(so,skillDamage);
            
            OnChangeWeapon?.Invoke(so);
        }
        public void SetCharacter(ForCharacterUI forUI,CharacterHpUI hp = null,EntitySO character = null)
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
            
            //ChangeWeapon(weapon);
            hpUI.SetCharacter(entity.EntityName);
            hpUI.UpdateHp(curHp, maxHp);
            
            _forCharacter = forUI;
            _forCharacter.SetHpUI(curHp, maxHp,entity.EntityName);
        }

        private void SetStats()
        {
            if(_isSetStat) return;
            Dictionary<StatsType, int> stats = HouseManager.Instance.PlayerStat.characterStats[entity.EntityName].ToDictionary();
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
                    if(target.outline.color != outlineColor && target.outline.color != new Color(0,0,0,0))
                        target.outline.color = (target.outline.color * 2) - outlineColor;
                    else
                        target.outline.color = new Color(0,0,0,0);
                    
                    canTargets.Add(target);
                    targets.Remove(target);
                }
                return;
            }
            
            if ((!isGlobal && targets.Count > 0) || targets.Count >= maxGlobal) //가능한 타겟수 찼다면 맨 처음을 변경
            {
                if(targets[0].outline.color != outlineColor && targets[0].outline.color != new Color(0,0,0,0))
                    targets[0].outline.color = (targets[0].outline.color * 2) - outlineColor;
                else
                    targets[0].outline.color = new Color(0,0,0,0);
                canTargets.Add(targets[0]);
                targets.RemoveAt(0);
            }

            targets.Add(target);
            canTargets.Remove(target);
            if(target.outline.color != new Color(0,0,0,0))
                target.outline.color = Color.Lerp(target.outline.color, outlineColor, 0.5f);
            else
            {
                target.outline.color += outlineColor;
            }
        }
        #endregion

        #region Entity
        protected override void Update()
        {
            base.Update();
            if (curSkillDelay > skillAttackDelay)
            {
                curSkillDelay = skillAttackDelay;
            }
            
            if(_forCharacter)
                _forCharacter.CurSkill(curSkillDelay,skillAttackDelay);
        }
        public override void UseSkill()
        {
            if(curSkillDelay < skillAttackDelay || skillAttackDelay < 0) return;
            //무기 내구도 감소
            float weaponDamage = _baseWeaponDemage - ((_baseWeaponDemage / 100) *
                                                      (_stats[StatsType.defense]/2 + _stats[StatsType.skill]/2));
            base.UseSkill();

            OnSkillWeapon?.Invoke(_useWeapon, weaponDamage);
        }
        public override void Hit(float damage)
        {
            base.Hit(damage);
            hpUI.UpdateHp(curHp, maxHp);
            _forCharacter.SetHpUI(curHp, maxHp);
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