using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.UI.Job;
using _02Script.Battle.UI.Weapon;
using _02Script.Collect.Item;
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
    public class BattleInventory: LoadInventoryManager
    {
        protected SerializedDictionary<ItemType, WeaponItemDataSO> _allWeaponDataSO = new SerializedDictionary<ItemType, WeaponItemDataSO>();
        
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
            
            if(BattleSaveManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
            CollectItem.OnGetItem += GetItem;
            WeaponInventory.OnDeleteWeapon += DeleteWeapon;
        }

        protected override void OnDisable()
        {
            base.OnDisable(); 
            WeaponArmorStartGiveItem.OnGetBuff -= AddItem;
            CollectItem.OnGetItem -= GetItem;
            WeaponInventory.OnDeleteWeapon -= DeleteWeapon;
            CompoundResult.OnGetItem -= AddItem;
            CompoundResult.OnUseItem -= ThrowItem;
            SelectDistribution.OnStart -= LoadItem;
        }
        #endregion
        protected override void LoadItem() //불러오기
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

        protected virtual void DeleteWeapon(WeaponInventoryCard weapon) //무기 정보 소멸 및 삭제
        {
            ItemData data = weapon.ReturnData();
            GameObject soonDelete = null;
            
            if(!ItemDatas.ContainsKey(data.ReturnDataSO())) return;
            
            foreach (ItemCard w in ItemCards[ItemDatas[data.ReturnDataSO()]])
            {
                if(w != weapon) continue;
                soonDelete = w.gameObject;
                break;
            }
            
            ItemCards[ItemDatas[data.ReturnDataSO()]].Remove(weapon);
            if (ItemCards[ItemDatas[data.ReturnDataSO()]].Count <= 0) //무기 다 사라지면 없애버리기
                ItemDatas.Remove(data.ReturnDataSO());
            
            Destroy(soonDelete);
        }

        #region GetAddItem (inventory)
        protected virtual void GetItem(ItemDataSO data, int count, EntityName type) //아이템 얻은거, 카드도 생성
        {
            if (data.category != ItemCategory.weapon && 
                data.category != ItemCategory.armor ) return;
            WeaponArmorSaveData save = null;
            foreach (WeaponArmorSaveData saveData in BattleSaveManager.Instance.PlayerStat.weaponArmor[data.itemType].ToArray())
            {
                if (saveData.hp == count)
                {
                    save = saveData;
                    BattleSaveManager.Instance.PlayerStat.weaponArmor[data.itemType].Remove(saveData);//hp가 겹칠 때 계속 해당 데이터만 참고하게 될테니, 또한 전투하면서 hp 계속 변경 될거니까
                }
            }

            AddItem(data,save,count);
        }
        public override void AddItem(ItemDataSO item,WeaponArmorSaveData saveData, int count = 1)
        {
            switch(item.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    if (!ItemDatas.ContainsKey(item))
                        NewCard(item, false, 0,0);
                    break;

                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    NewCard(item, ItemDatas.ContainsKey(item), count, count,saveData);
                    break;
            }

            ItemData data = ItemDatas[item];
            
            ItemCard card = ItemCards[data][ItemCards[data].Count -1]; //갓 생성
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