using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.Buff;
using _02Script.Battle.Entity;
using _02Script.Battle.UI.Job;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using _02Script.UI.Save;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    // [순수 뷰용 인벤토리]
    public class WeaponInventory : BattleInventory
    {
        public static new Action<WeaponInventoryCard> OnDeleteWeapon;
        
        [Header("WeaponInventory (View Only)")]
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
            
            WeaponInventoryCard.OnMouseClick += CloseWeaponInventory;
            BattleCharacter.OnSkillWeapon += WeaponDamage;
            BattleInventory.OnDurabilityChanged += HandleDurabilityChanged;
            BattleInventory.OnEquipmentDestroyed += HandleEquipmentDestroyed;
            SelectDistribution.OnStart += LoadItem;
            LoadCard.OnLoad += LoadItem;

            if (BattleSaveManager.Instance != null && BattleSaveManager.Instance.isStart && ItemCards.Count <= 0)
            {
                LoadItem();
            }
        }

        protected override void OnDisable()
        {
            WeaponInventoryCard.OnMouseClick -= CloseWeaponInventory;
            BattleCharacter.OnSkillWeapon -= WeaponDamage;
            BattleInventory.OnDurabilityChanged -= HandleDurabilityChanged;
            BattleInventory.OnEquipmentDestroyed -= HandleEquipmentDestroyed;
            SelectDistribution.OnStart -= LoadItem;
            LoadCard.OnLoad -= LoadItem;
        }
        #endregion

        private void CloseWeaponInventory(WeaponInventoryCard _w, List<BuffSO> _b, EntityName _e, WeaponArmorSaveData _d)
        {
            inventoryWindow.gameObject.SetActive(false);
        }

        #region View Load (뷰 전용 표시)
        protected override void LoadItem()
        {
            SettingAllDataSO();
            if (inventoryCharacter == null) return;

            var stat = BattleSaveManager.Instance.PlayerStat;
            if (stat == null) return;

            Dictionary<ItemType, List<float>> save = stat.items.ToDictionary();
            Dictionary<ItemType, List<WeaponArmorSaveData>> etcData = stat.weaponArmor.ToDictionary();
            
            foreach (var cardList in ItemCards.Values)
            foreach (var card in cardList)
                if (card != null) Destroy(card.gameObject);
            ItemCards.Clear();
            ItemDatas.Clear();

            foreach (KeyValuePair<ItemType, List<float>> item in save.ToList())
            {
                if (item.Key == ItemType.notting) continue;
                if (!_allWeaponDataSO.ContainsKey(item.Key)) continue;

                WeaponItemDataSO weaponSO = _allWeaponDataSO[item.Key];
                if (!inventoryCharacter.ReturnSO().useWeapons.Contains(weaponSO)) continue;

                int count = item.Value.Count;
                for (int i = 1; i < count; i++)
                {
                    WeaponArmorSaveData saveData = null;
                    if (etcData.ContainsKey(item.Key) && etcData[item.Key].Count >= i)
                        saveData = etcData[item.Key][i - 1];
                    else
                        saveData = NewSaveData(weaponSO, (int)item.Value[i]);

                    CreateViewCard(weaponSO, saveData, item.Value[i]);
                }
            }

            AutoWeaponSelect();
        }

        private void CreateViewCard(WeaponItemDataSO so, WeaponArmorSaveData saveData, float hp)
        {
            if (!itemInventory.ContainsKey(so.category)) return;

            ItemData itemData;
            if (!ItemDatas.ContainsKey(so))
            {
                itemData = new ItemData();
                itemData.NewItem(so);
                ItemDatas.Add(so, itemData);
            }
            else
            {
                itemData = ItemDatas[so];
            }

            Transform parent = itemInventory[so.category];
            ItemCard newCard = Instantiate(cardPrefab, parent);
            newCard.gameObject.SetActive(true);

            WeaponInventoryCard weaponCard = newCard as WeaponInventoryCard;
            if (weaponCard != null)
            {
                weaponCard.Set(_weaponEntity);
                weaponCard.NewCard(buffFind, itemData, 0, (int)hp, saveData);
            }

            if (!ItemCards.ContainsKey(itemData))
            {
                ItemCards.Add(itemData, new List<ItemCard>());
            }
            ItemCards[itemData].Add(newCard);
            itemData.AddCountOnly();
            newCard.UpdateCountUI();
        }
        #endregion

        #region [핵심] 아이템 추가 기능 박살 (뷰용으로 변경하여 복제 원천 차단)
        public override void AddItem(ItemDataSO item, WeaponArmorSaveData saveData, int count = 1)
        {
        }

        public override void AddItem(ItemDataSO item, int count = 1)
        {
        }
        #endregion

        #region WeaponDamage & Event Sync (이벤트 기반 실시간 전체 동기화)
        private void WeaponDamage(WeaponInventoryCard weapon, float minus)
        {
            if (!weapon) return;
            if (weapon.GetEntity() != _weaponEntity && weapon.GetEntity() != EntityName.None) return;

            ItemDataSO so = weapon.ReturnData()?.ReturnDataSO();
            if (so == null) return;

            float oldHp = weapon.ReturnNum(false);

            if (BattleInventory.Instance != null)
            {
                BattleInventory.Instance.ConsumeItemDamage(
                    so.itemType,
                    oldHp,
                    minus,
                    weapon.ReturnSaveData()
                );
            }

            if (weapon.ReturnNum(false) - minus <= 0)
            {
                WarringManager.Warring.ShowWarring(
                    $"{EnumToString.Name(so.itemType)}의 내구력이 다하여 부셔졌습니다.");

                if (BattleInventory.Instance != null)
                {
                    BattleInventory.Instance.DeleteWeapon(weapon);
                }
            }
        }

        private void HandleDurabilityChanged(ItemType type, WeaponArmorSaveData data, float newHp)
        {
            if (!_allWeaponDataSO.ContainsKey(type)) return;
            WeaponItemDataSO so = _allWeaponDataSO[type];

            if (!ItemDatas.ContainsKey(so)) return;
            ItemData itemData = ItemDatas[so];
            if (!ItemCards.ContainsKey(itemData)) return;

            foreach (ItemCard card in ItemCards[itemData])
            {
                WeaponInventoryCard weaponCard = card as WeaponInventoryCard;
                if (weaponCard == null) continue;

                if (data != null && weaponCard.ReturnSaveData() == data)
                {
                    weaponCard.ItemDamage(weaponCard.ReturnNum(false) - newHp);
                    weaponCard.UpdateCountUI();
                    break;
                }
                else if (Mathf.Abs(weaponCard.ReturnNum(false) - newHp) > 0.01f)
                {
                    weaponCard.ItemDamage(weaponCard.ReturnNum(false) - newHp);
                    weaponCard.UpdateCountUI();
                    break;
                }
            }
        }

        private void HandleEquipmentDestroyed(ItemType type, WeaponArmorSaveData data)
        {
            if (!_allWeaponDataSO.ContainsKey(type)) return;
            WeaponItemDataSO so = _allWeaponDataSO[type];

            if (!ItemDatas.ContainsKey(so)) return;
            ItemData itemData = ItemDatas[so];
            if (!ItemCards.ContainsKey(itemData)) return;

            WeaponInventoryCard target = null;
            foreach (ItemCard card in ItemCards[itemData])
            {
                WeaponInventoryCard weaponCard = card as WeaponInventoryCard;
                if (weaponCard == null) continue;

                if (data != null && weaponCard.ReturnSaveData() == data)
                {
                    target = weaponCard;
                    break;
                }
                else if (weaponCard.ReturnNum(false) <= 0)
                {
                    target = weaponCard;
                    break;
                }
            }

            if (target != null)
            {
                RemoveViewCard(target);
                AutoWeaponSelect();
            }
        }

        private void RemoveViewCard(WeaponInventoryCard card)
        {
            if (card == null) return;
            ItemData data = card.ReturnData();
            if (data == null || data.ReturnDataSO() == null) return;

            if (ItemDatas.ContainsKey(data.ReturnDataSO()))
            {
                ItemCards[data].Remove(card);
                if (ItemCards[data].Count <= 0)
                {
                    ItemDatas.Remove(data.ReturnDataSO());
                }
            }
            Destroy(card.gameObject);
        }
        #endregion

        #region Set
        protected override void SettingAllDataSO()
        {
            if (inventoryCharacter == null) return;
            
            inventoryText.text = EnumToString.Name(inventoryCharacter.ReturnSO().EntityName) + "의 무기 변경";
            
            _allWeaponDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                if (inventoryCharacter.ReturnSO().useWeapons.Contains(data as WeaponItemDataSO))
                    _allWeaponDataSO.Add(data.itemType, data as WeaponItemDataSO);
            }
        }

        public void SetInventoryCharacter(BattleCharacter character)
        {
            inventoryCharacter = character;
            _weaponEntity = character.ReturnName();
            SettingAllDataSO();
            LoadItem();
            AutoWeaponSelect();
        }

        private void AutoWeaponSelect()
        {
            if (ItemCards.Count <= 0 || ItemCards.First().Value.Count <= 0)
            {
                _isBeforeAutoChange = true;
                if (inventoryCharacter != null)
                {
                    inventoryCharacter.ChangeWeapon(null, null, _weaponEntity, null);
                }
                return;
            }
            _isBeforeAutoChange = false;
            WeaponInventoryCard weapon = ItemCards.First().Value[0] as WeaponInventoryCard;
            if (weapon == null) return;
            
            List<BuffSO> buff = new List<BuffSO>();
            WeaponArmorSaveData saveData = weapon.ReturnSaveData();

            if (saveData != null && saveData.buffTypes != null)
            {
                foreach (BuffType b in saveData.buffTypes)
                {
                    var buffSO = buffFind.GetBuff(b);
                    if (buffSO != null) buff.Add(buffSO);
                }
            }
            
            if (inventoryCharacter != null)
            {
                inventoryCharacter.ChangeWeapon(weapon, buff, _weaponEntity, saveData);
            }
        }
        #endregion
    }
}
