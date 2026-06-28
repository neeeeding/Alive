using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.Buff;
using _02Script.Battle.Entity;
using _02Script.Battle.UI.Job;
using _02Script.Battle.UI.Weapon;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using _02Script.UI.Save;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.UI.Armor
{
    // [순수 뷰용 인벤토리]
    public class ArmorInventory : LoadInventoryManager
    {
        public static Action<ArmorInventoryCard> OnDeleteArmor;
        
        [Header("ArmorInventory (View Only)")]
        [SerializeField] private BattleCharacter inventoryCharacter;

        [Header("Need")]
        [SerializeField] private GameObject inventoryWindow;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private BuffFind buffFind;
        
        private SerializedDictionary<ItemType, ArmorItemDataSO> _allArmorDataSO = new SerializedDictionary<ItemType, ArmorItemDataSO>();
        private EntityName _armorEntity;

        #region EnDiAw
        protected override void OnEnable()
        {
            BattleSaveManager.OnStart += LoadItem;
            LoadCard.OnLoad += LoadItem;
            SelectDistribution.OnStart += LoadItem;
            BattleInventory.OnDurabilityChanged += HandleDurabilityChanged;
            BattleInventory.OnEquipmentDestroyed += HandleEquipmentDestroyed;
            
            if (BattleSaveManager.Instance != null && BattleSaveManager.Instance.isStart && ItemCards.Count <= 0)
            {
                LoadItem();
            }
            ArmorInventoryCard.OnMouseClick += CloseWeaponInventory;
            BattleCharacter.OnUseArmor += ArmorDamage;
            ArmorInventory.OnDeleteArmor += DeleteArmor;
        }

        protected override void OnDisable()
        {
            BattleSaveManager.OnStart -= LoadItem;
            LoadCard.OnLoad -= LoadItem;
            SelectDistribution.OnStart -= LoadItem;
            BattleInventory.OnDurabilityChanged -= HandleDurabilityChanged;
            BattleInventory.OnEquipmentDestroyed -= HandleEquipmentDestroyed;
            
            ArmorInventoryCard.OnMouseClick -= CloseWeaponInventory;
            BattleCharacter.OnUseArmor -= ArmorDamage;
            ArmorInventory.OnDeleteArmor -= DeleteArmor;
        }
        #endregion

        private void CloseWeaponInventory(ArmorInventoryCard _a, List<BuffSO> _b, EntityName _e, WeaponArmorSaveData _d)
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
                if (!_allArmorDataSO.ContainsKey(item.Key)) continue;

                ArmorItemDataSO armorSO = _allArmorDataSO[item.Key];

                int count = item.Value.Count;
                for (int i = 1; i < count; i++)
                {
                    WeaponArmorSaveData saveData = null;
                    if (etcData.ContainsKey(item.Key) && etcData[item.Key].Count >= i)
                        saveData = etcData[item.Key][i - 1];
                    else
                        saveData = NewSaveData(armorSO, (int)item.Value[i]);

                    CreateViewCard(armorSO, saveData, item.Value[i]);
                }
            }

            AutoArmorSelect();
        }

        private void CreateViewCard(ArmorItemDataSO so, WeaponArmorSaveData saveData, float hp)
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

            ArmorInventoryCard armorCard = newCard as ArmorInventoryCard;
            if (armorCard != null)
            {
                armorCard.Set(_armorEntity);
                armorCard.NewCard(buffFind, itemData, 0, (int)hp, saveData);
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

        #region ArmorDamage & Event Sync (이벤트 기반 실시간 전체 동기화)
        private void ArmorDamage(ArmorInventoryCard armor)
        {
            if (!armor) return;
            if (armor.GetEntity() != _armorEntity && armor.GetEntity() != EntityName.None) return;

            ItemDataSO so = armor.ReturnData()?.ReturnDataSO();
            if (so == null) return;

            float minus = (so as ArmorItemDataSO).damage;
            float oldHp = armor.ReturnNum(false);

            if (BattleInventory.Instance != null)
            {
                BattleInventory.Instance.ConsumeItemDamage(
                    so.itemType,
                    oldHp,
                    minus,
                    armor.ReturnSaveData()
                );
            }

            if (armor.ReturnNum(false) - minus <= 0)
            {
                WarringManager.Warring.ShowWarring(
                    $"{EnumToString.Name(so.itemType)}의 내구력이 다하여 부셔졌습니다.");

                OnDeleteArmor?.Invoke(armor);
                DeleteArmor(armor);
            }
        }

        private void HandleDurabilityChanged(ItemType type, WeaponArmorSaveData data, float newHp)
        {
            if (!_allArmorDataSO.ContainsKey(type)) return;
            ArmorItemDataSO so = _allArmorDataSO[type];

            if (!ItemDatas.ContainsKey(so)) return;
            ItemData itemData = ItemDatas[so];
            if (!ItemCards.ContainsKey(itemData)) return;

            foreach (ItemCard card in ItemCards[itemData])
            {
                ArmorInventoryCard armorCard = card as ArmorInventoryCard;
                if (armorCard == null) continue;

                if (data != null && armorCard.ReturnSaveData() == data)
                {
                    armorCard.ItemDamage(armorCard.ReturnNum(false) - newHp);
                    armorCard.UpdateCountUI();
                    break;
                }
                else if (Mathf.Abs(armorCard.ReturnNum(false) - newHp) > 0.01f)
                {
                    armorCard.ItemDamage(armorCard.ReturnNum(false) - newHp);
                    armorCard.UpdateCountUI();
                    break;
                }
            }
        }

        private void HandleEquipmentDestroyed(ItemType type, WeaponArmorSaveData data)
        {
            if (!_allArmorDataSO.ContainsKey(type)) return;
            ArmorItemDataSO so = _allArmorDataSO[type];

            if (!ItemDatas.ContainsKey(so)) return;
            ItemData itemData = ItemDatas[so];
            if (!ItemCards.ContainsKey(itemData)) return;

            ArmorInventoryCard target = null;
            foreach (ItemCard card in ItemCards[itemData])
            {
                ArmorInventoryCard armorCard = card as ArmorInventoryCard;
                if (armorCard == null) continue;

                if (data != null && armorCard.ReturnSaveData() == data)
                {
                    target = armorCard;
                    break;
                }
                else if (armorCard.ReturnNum(false) <= 0)
                {
                    target = armorCard;
                    break;
                }
            }

            if (target != null)
            {
                DeleteArmor(target);
                AutoArmorSelect();
            }
        }

        private void DeleteArmor(ArmorInventoryCard armor)
        {
            if (armor == null) return;
            ItemData data = armor.ReturnData();
            if (data == null || data.ReturnDataSO() == null) return;

            ItemType itemType = data.ReturnDataSO().itemType;
            float hp = armor.ReturnNum(false);
            WeaponArmorSaveData saveData = armor.ReturnSaveData();

            var stat = SaveManagerCheck.GetCurScenePlayerStat();
            if (stat != null)
            {
                if (stat.items != null && stat.items.ContainsKey(itemType))
                {
                    for (int i = 1; i < stat.items[itemType].Count; i++)
                    {
                        if (Mathf.Abs(stat.items[itemType][i] - hp) < 1f || stat.items[itemType][i] <= 0)
                        {
                            stat.items[itemType].RemoveAt(i);
                            break;
                        }
                    }
                    stat.items.SyncListFromDict();
                }
                if (stat.weaponArmor != null && stat.weaponArmor.ContainsKey(itemType))
                {
                    if (saveData != null)
                    {
                        stat.weaponArmor[itemType].Remove(saveData);
                    }
                    else
                    {
                        stat.weaponArmor[itemType].RemoveAll(w => w == null || Mathf.Abs(w.hp - hp) < 1f || w.hp <= 0);
                    }
                    stat.weaponArmor.SyncListFromDict();
                }
            }

            if (ItemDatas.ContainsKey(data.ReturnDataSO()))
            {
                ItemCards[data].Remove(armor);
                if (ItemCards[data].Count <= 0)
                    ItemDatas.Remove(data.ReturnDataSO());
            }
            
            Destroy(armor.gameObject);
        }
        #endregion

        #region Set
        protected override void SettingAllDataSO()
        {
            if (inventoryCharacter == null) return;
            
            inventoryText.text = EnumToString.Name(inventoryCharacter.ReturnSO().EntityName) + "의 갑옷 변경";
            
            _allArmorDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                _allArmorDataSO.Add(data.itemType, data as ArmorItemDataSO);
            }
        }

        public void SetInventoryCharacter(BattleCharacter character)
        {
            inventoryCharacter = character;
            _armorEntity = character.ReturnName();
            SettingAllDataSO();
            LoadItem();
            AutoArmorSelect();
        }

        private void AutoArmorSelect()
        {
            if (ItemCards.Count <= 0 || ItemCards.First().Value.Count <= 0)
            {
                if (inventoryCharacter != null)
                {
                    inventoryCharacter.ChangeArmor(null, null, _armorEntity, null);
                }
                return;
            }
            ArmorInventoryCard armor = ItemCards.First().Value[0] as ArmorInventoryCard;
            if (armor == null) return;

            List<BuffSO> buff = new List<BuffSO>();
            WeaponArmorSaveData saveData = armor.ReturnSaveData();

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
                inventoryCharacter.ChangeArmor(armor, buff, _armorEntity, saveData);
            }
        }
        #endregion
    }
}
