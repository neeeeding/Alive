using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.UI.Job;
using _02Script.Battle.UI.Weapon;
using _02Script.Collect.Item;
using _02Script.Etc;
using _02Script.Farming;
using _02Script.Inventory.Etc;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using _02Script.Produce.Weapon.Compound;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Save;
using _02Script.UI.Store;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Battle
{
    // [Master 인벤토리] 씬에 단 1개만 존재하며, 모든 실제 아이템 데이터와 세이브를 총괄 관리합니다.
    public class BattleInventory : LoadInventoryManager
    {
        public static BattleInventory Instance { get; private set; }

        // [이벤트 연동] 부모(전투 인벤토리)와 모든 자식(무기/갑옷 인벤토리) 간 실시간 동기화 이벤트
        public static Action<ItemType, WeaponArmorSaveData, float> OnDurabilityChanged;
        public static Action<ItemType, WeaponArmorSaveData> OnEquipmentDestroyed;

        protected SerializedDictionary<ItemType, WeaponItemDataSO> _allWeaponDataSO = new SerializedDictionary<ItemType, WeaponItemDataSO>();
        
        protected virtual void Awake()
        {
            Instance = this;
        }

        #region EnDiAw
        protected override void OnEnable()
        {
            DialogItem.OnGetItem += GetOrThrowItem;
            WeaponArmorStartGiveItem.OnGetBuff += AddItem;
            InGameItem.OnGetItem += AddItem;
            Field.OnGetViand += AddItem;
            GameEvent.GameEvent.OnGetItem += AddItem;
            StoreCard.OnSellItem += AddItem;
            StoreCard.OnPayItem += ThrowItem;
            Field.OnUseSeed += ThrowItem;
            CompoundResult.OnGetItem += AddItem;
            CompoundResult.OnUseItem += ThrowItem;
            SelectDistribution.OnStart += LoadItem;
            
            LoadCard.OnLoad += LoadItem;

            // [추가] 부모 전투 인벤토리 본인도 실시간 내구도 변경/파괴 이벤트를 구독하여 UI 카드 즉시 갱신
            OnDurabilityChanged += HandleDurabilityChanged;
            OnEquipmentDestroyed += HandleEquipmentDestroyed;
            
            if (BattleSaveManager.Instance != null && BattleSaveManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
            CollectItem.OnGetItem += GetItem;
            WeaponInventory.OnDeleteWeapon += DeleteWeapon;
        }

        protected override void OnDisable()
        {
            base.OnDisable(); 
            DialogItem.OnGetItem -= GetOrThrowItem;
            WeaponArmorStartGiveItem.OnGetBuff -= AddItem;
            InGameItem.OnGetItem -= AddItem;
            Field.OnGetViand -= AddItem;
            GameEvent.GameEvent.OnGetItem -= AddItem;
            StoreCard.OnSellItem -= AddItem;
            StoreCard.OnPayItem -= ThrowItem;
            Field.OnUseSeed -= ThrowItem;
            CollectItem.OnGetItem -= GetItem;
            WeaponInventory.OnDeleteWeapon -= DeleteWeapon;
            CompoundResult.OnGetItem -= AddItem;
            CompoundResult.OnUseItem -= ThrowItem;
            SelectDistribution.OnStart -= LoadItem;
            LoadCard.OnLoad -= LoadItem;

            OnDurabilityChanged -= HandleDurabilityChanged;
            OnEquipmentDestroyed -= HandleEquipmentDestroyed;
        }
        #endregion
        
        protected override void LoadItem()
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = BattleSaveManager.Instance.PlayerStat.items.ToDictionary();
            Dictionary<ItemType, List<WeaponArmorSaveData>> etcData = BattleSaveManager.Instance.PlayerStat.weaponArmor.ToDictionary();
            
            foreach (var cardList in ItemCards.Values)
            foreach (var card in cardList)
                if (card != null) Destroy(card.gameObject);
            ItemCards.Clear();
            ItemDatas.Clear();

            foreach (KeyValuePair<ItemType, List<float>> item in save.ToList())
            {
                if (item.Key == ItemType.notting) continue;
                if (_allWeaponDataSO == null)
                {
                    SettingAllDataSO();
                }
                
                if (!_allWeaponDataSO.ContainsKey(item.Key)) continue;

                ItemDataSO so = _allWeaponDataSO[item.Key];

                LoadItem(item, etcData, so);
            }
        }

        // [핵심] 내구도 소모 + 세이브 동기화 + 모든 뷰로 이벤트 브로드캐스팅
        public virtual void ConsumeItemDamage(ItemType itemType, float oldHp, float damage, WeaponArmorSaveData saveData)
        {
            float newHp = Mathf.Max(0, oldHp - damage);
            var stat = SaveManagerCheck.GetCurScenePlayerStat();
            if (stat == null) return;

            // 1. PlayerStat.items float 리스트 갱신
            if (stat.items != null && stat.items.ContainsKey(itemType))
            {
                var list = stat.items[itemType];
                int targetIdx = -1;
                float minDiff = float.MaxValue;
                for (int i = 1; i < list.Count; i++)
                {
                    float diff = Mathf.Abs(list[i] - oldHp);
                    if (diff < minDiff)
                    {
                        minDiff = diff;
                        targetIdx = i;
                    }
                }
                if (targetIdx > 0)
                {
                    list[targetIdx] = newHp;
                }
                stat.items.SyncListFromDict();
            }

            // 2. PlayerStat.weaponArmor 상세 데이터 갱신
            if (stat.weaponArmor != null && stat.weaponArmor.ContainsKey(itemType))
            {
                var waList = stat.weaponArmor[itemType];
                if (saveData != null && waList.Contains(saveData))
                {
                    saveData.hp = newHp;
                }
                else
                {
                    var target = waList.FirstOrDefault(w => w != null && Mathf.Abs(w.hp - oldHp) < 1f);
                    if (target != null)
                    {
                        target.hp = newHp;
                    }
                }
                stat.weaponArmor.SyncListFromDict();
            }

            // 3. [이벤트 전파] 부모(전투 인벤토리) 및 모든 자식(무기/갑옷 인벤토리)에 실시간 내구도 변경 알림
            OnDurabilityChanged?.Invoke(itemType, saveData, newHp);
        }

        // [부모 전투 인벤토리 실시간 UI 갱신 핸들러]
        protected virtual void HandleDurabilityChanged(ItemType type, WeaponArmorSaveData data, float newHp)
        {
            if (!_allWeaponDataSO.ContainsKey(type)) return;
            WeaponItemDataSO so = _allWeaponDataSO[type];

            if (!ItemDatas.ContainsKey(so)) return;
            ItemData itemData = ItemDatas[so];
            if (!ItemCards.ContainsKey(itemData)) return;

            foreach (ItemCard card in ItemCards[itemData])
            {
                if (card == null) continue;

                if (data != null && card.ReturnSaveData() == data)
                {
                    card.ItemDamage(card.ReturnNum(false) - newHp);
                    card.UpdateCountUI();
                    break;
                }
                else if (Mathf.Abs(card.ReturnNum(false) - newHp) > 0.01f)
                {
                    card.ItemDamage(card.ReturnNum(false) - newHp);
                    card.UpdateCountUI();
                    break;
                }
            }
        }

        // [부모 전투 인벤토리 장비 파괴 UI 갱신 핸들러]
        protected virtual void HandleEquipmentDestroyed(ItemType type, WeaponArmorSaveData data)
        {
            if (!_allWeaponDataSO.ContainsKey(type)) return;
            WeaponItemDataSO so = _allWeaponDataSO[type];

            if (!ItemDatas.ContainsKey(so)) return;
            ItemData itemData = ItemDatas[so];
            if (!ItemCards.ContainsKey(itemData)) return;

            ItemCard target = null;
            foreach (ItemCard card in ItemCards[itemData])
            {
                if (card == null) continue;

                if (data != null && card.ReturnSaveData() == data)
                {
                    target = card;
                    break;
                }
                else if (card.ReturnNum(false) <= 0)
                {
                    target = card;
                    break;
                }
            }

            if (target != null)
            {
                ItemCards[itemData].Remove(target);
                if (ItemCards[itemData].Count <= 0)
                {
                    ItemDatas.Remove(so);
                }
                Destroy(target.gameObject);
            }
        }

        // 무기 파괴 시 마스터 세이브 및 UI에서 완전 제거
        public virtual void DeleteWeapon(WeaponInventoryCard weapon)
        {
            if (weapon == null) return;
            ItemData data = weapon.ReturnData();
            if (data == null || data.ReturnDataSO() == null) return;
            
            ItemType itemType = data.ReturnDataSO().itemType;
            float hp = weapon.ReturnNum(false);
            WeaponArmorSaveData saveData = weapon.ReturnSaveData();

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

            // [이벤트 전파] 모든 뷰에 장비 파괴 알림
            OnEquipmentDestroyed?.Invoke(itemType, saveData);

            GameObject soonDelete = null;
            if (ItemDatas.ContainsKey(data.ReturnDataSO()))
            {
                foreach (ItemCard w in ItemCards[ItemDatas[data.ReturnDataSO()]])
                {
                    if (w == weapon)
                    {
                        soonDelete = w.gameObject;
                        break;
                    }
                }
                
                ItemCards[ItemDatas[data.ReturnDataSO()]].Remove(weapon);
                if (ItemCards[ItemDatas[data.ReturnDataSO()]].Count <= 0)
                    ItemDatas.Remove(data.ReturnDataSO());
            }
            
            if (soonDelete != null)
                Destroy(soonDelete);
        }

        #region GetAddItem (inventory)
        protected virtual void GetItem(ItemDataSO data, int count, EntityName type)
        {
            if (data.category != ItemCategory.weapon && 
                data.category != ItemCategory.armor) return;
            WeaponArmorSaveData save = null;
            if (BattleSaveManager.Instance.PlayerStat.weaponArmor.ContainsKey(data.itemType))
            {
                foreach (WeaponArmorSaveData saveData in BattleSaveManager.Instance.PlayerStat.weaponArmor[data.itemType].ToArray())
                {
                    if (Mathf.Abs(saveData.hp - count) < 1f)
                    {
                        save = saveData;
                        BattleSaveManager.Instance.PlayerStat.weaponArmor[data.itemType].Remove(saveData);
                        break;
                    }
                }
            }

            AddItem(data, save, count);
        }

        public override void AddItem(ItemDataSO item, WeaponArmorSaveData saveData, int count = 1)
        {
            switch (item.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    if (!ItemDatas.ContainsKey(item))
                        NewCard(item, false, 0, 0);
                    break;

                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    NewCard(item, ItemDatas.ContainsKey(item), count, count, saveData);
                    break;
            }

            ItemData data = ItemDatas[item];
            
            ItemCard card = ItemCards[data][ItemCards[data].Count - 1];
            card.gameObject.SetActive(true);
            card.UpdateCountUI();
        }
        #endregion
        
        protected override void SettingAllDataSO()
        {
            _allWeaponDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                _allWeaponDataSO.Add(data.itemType, data as WeaponItemDataSO);
            }
        }
    }
}
