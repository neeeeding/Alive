using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.Buff;
using _02Script.Battle.Entity;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponInventory : BattleInventory
    {
        public static Action<WeaponInventoryCard> OnDeleteWeapon;
        
        [Header("WeaponInventory")]
        [SerializeField] private BattleCharacter inventoryCharacter;

        [Header("Need")]
        [SerializeField] private GameObject inventoryWindow;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private BuffFind buffFind;
        
        private EntityName _weaponEntity;
        private bool _isBeforeAutoChange;

        #region EnDiAw
        protected override void OnEnable()
        {
            _isBeforeAutoChange = true;
            base.OnEnable();
            
            WeaponInventoryCard.OnMouseClick += CloseWeaponInventory;
            BattleCharacter.OnSkillWeapon += WeaponDamage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            WeaponInventoryCard.OnMouseClick -= CloseWeaponInventory;
            BattleCharacter.OnSkillWeapon -= WeaponDamage;
        }
        #endregion

        private void CloseWeaponInventory(WeaponInventoryCard _w,List<BuffSO> _b, EntityName _e,WeaponArmorSaveData _d)
        {
            inventoryWindow.gameObject.SetActive(false);
        }

        #region WeaponDamage
            //데미지 감소
            private void WeaponDamage(WeaponInventoryCard weapon, float minus)
            {
                if(!weapon) return;
                if (!ItemDatas.ContainsKey(weapon.ReturnData().ReturnDataSO())) return;
                ItemData data = ItemDatas[weapon.ReturnData().ReturnDataSO()];
                
                WeaponInventoryCard useCard = weapon;
                foreach (ItemCard d in ItemCards[data]) //다른 인벤토리에서는 '카드'자체의 인스턴스 값이 달라 적용이 안되므로
                {
                    if(d.ReturnNum(false) != weapon.ReturnNum(false)) continue;
                    useCard = d as WeaponInventoryCard;
                    break;
                }

                //data.UseItem(weapon.ReturnNum(false),minus, true); //데미지 삭제 (해봤자 아래 때문에 데미지 두 번 삭제됨)
                useCard.ItemDamage(minus);
                useCard.UpdateCountUI();
                    
                if(useCard.ReturnNum(false) > 0) return; //내구도 0 이하시 삭제 시작
                WarringManager.Warring.ShowWarring(
                    $"{EnumToString.Name(useCard.ReturnData().ReturnDataSO().itemType)}의 내구력이 다하여 부셔졌습니다.");
                
                OnDeleteWeapon?.Invoke(useCard);
                AutoWeaponSelect();
            }
        #endregion

        #region GetAddItem (inventory)
        public override void AddItem(ItemDataSO item,WeaponArmorSaveData saveData, int count = 1)
        {
            if(item.category != ItemCategory.weapon) return;
            
            if(_allWeaponDataSO.Count <= 0)
                SettingAllDataSO();
            
            if (_allWeaponDataSO.ContainsKey(item.itemType))
            {
                base.AddItem(_allWeaponDataSO[item.itemType], count);
                
                ItemData d = ItemDatas[_allWeaponDataSO[item.itemType]]; //그냥 item하면 인스턴스 값이 달라서 못함.
                (ItemCards[d][ItemCards[d].Count -1] as WeaponInventoryCard).Set(_weaponEntity);
                
                if (item is WeaponItemDataSO)
                {
                    WeaponInventoryCard card = ItemCards[d][ItemCards[d].Count -1] as WeaponInventoryCard;
                    card.NewCard(buffFind,d,0,count,saveData);
                }
            }
            if (_isBeforeAutoChange)
            {
                AutoWeaponSelect();
            }
        }
        public override void AddItem(ItemDataSO item, int count = 1)
        {
            if(item.category != ItemCategory.weapon) return;
            
            if(_allWeaponDataSO.Count <= 0)
                SettingAllDataSO();
            
            if (_allWeaponDataSO.ContainsKey(item.itemType))
            {
                base.AddItem(_allWeaponDataSO[item.itemType], count);
                
                ItemData d = ItemDatas[_allWeaponDataSO[item.itemType]]; //그냥 item하면 인스턴스 값이 달라서 못함.
                (ItemCards[d][ItemCards[d].Count -1] as WeaponInventoryCard).Set(_weaponEntity);
                
                if (item is WeaponItemDataSO)
                {
                    WeaponInventoryCard card = ItemCards[d][ItemCards[d].Count -1] as WeaponInventoryCard;
                    WeaponArmorSaveData data = null;
                    card.NewCard(buffFind,d,0,count,data);
                }
            }
            if (_isBeforeAutoChange)
            {
                AutoWeaponSelect();
            }
        }

        protected override void LoadItem(KeyValuePair<ItemType, List<float>> item, Dictionary<ItemType, List<WeaponArmorSaveData>> etcData, ItemDataSO so)
        {
            int count = item.Value.Count;
            for (int i = 1; i < count; i++)
            {
                WeaponArmorSaveData saveData = null;
                if (etcData.ContainsKey(item.Key) && etcData[item.Key].Count >= i)
                    saveData = etcData[item.Key][i - 1];
                else
                    saveData = NewSaveData(so, (int)item.Value[i]);
                            
                AddItem(so,saveData, (int)item.Value[i]);
                //NewCard(so, ItemDatas.ContainsKey(so), (int)item.Value[i], (int)item.Value[i],saveData);
                if (!ItemDatas.ContainsKey(so))
                {
                    continue;
                }
                ItemData data = ItemDatas[so];
                data.AddCountOnly();
                ItemCards[data][ItemCards[data].Count - 1].UpdateCountUI();
            }
        }

        protected override void NewCard(ItemDataSO item, bool isEtc, int star = 3, int hp = 100, WeaponArmorSaveData saveData = null)
        {
            base.NewCard(item, isEtc, star, hp, saveData);
            ItemData itemData =ItemDatas[item];
            (ItemCards[itemData][ItemCards[itemData].Count - 1]as WeaponInventoryCard).Set(_weaponEntity);
            if (_isBeforeAutoChange)
            {
                AutoWeaponSelect();
            }
        }
        #endregion

        #region Set
        protected override void SettingAllDataSO()
        {
            if(inventoryCharacter == null) return;
            
            inventoryText.text = EnumToString.Name(inventoryCharacter.ReturnSO().EntityName)+"의 무기 변경";
            
            _allWeaponDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                if (inventoryCharacter.ReturnSO().useWeapons.Contains(data as WeaponItemDataSO))
                    _allWeaponDataSO.Add(data.itemType, data as WeaponItemDataSO);
            }
        }
        public void SetInventoryCharacter(BattleCharacter character) //누구의 인벤토리인지 지정해주기
        {
            inventoryCharacter = character;
            _weaponEntity = character.ReturnName();
            SettingAllDataSO();
            AutoWeaponSelect();
        }

        private void AutoWeaponSelect()
        {
            if (ItemCards.Count <= 0)
            {
                _isBeforeAutoChange = true;
                inventoryCharacter.ChangeWeapon(null,null,_weaponEntity,null);
                return;
            }
            _isBeforeAutoChange = false;
            WeaponInventoryCard weapon = ItemCards.First().Value[0] as WeaponInventoryCard;
            
            List<BuffSO> buff = new List<BuffSO>();
            
            Dictionary<ItemType, List<WeaponArmorSaveData>> dict = BattleSaveManager.Instance.PlayerStat.weaponArmor.ToDictionary();

            if (weapon == null ||dict == null ||
                dict.Count <= 0 ||
                !dict.ContainsKey(weapon.ReturnData().ReturnDataSO().itemType) ||
                dict[weapon.ReturnData().ReturnDataSO().itemType] == null ||
                dict[weapon.ReturnData().ReturnDataSO().itemType].Count <= 0 ||
                dict[weapon.ReturnData().ReturnDataSO().itemType][0] == null ||
                dict[weapon.ReturnData().ReturnDataSO().itemType][0].buffTypes == null)return;

            foreach (BuffType b in dict[weapon.ReturnData().ReturnDataSO().itemType][0].buffTypes)
            {
                buff.Add(buffFind.GetBuff(b));
            }
            
            inventoryCharacter.ChangeWeapon(weapon,buff,_weaponEntity,dict[weapon.ReturnData().ReturnDataSO().itemType][0]);
        }
        #endregion
    }
}