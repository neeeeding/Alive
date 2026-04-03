using System.Collections.Generic;
using System.Linq;
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
    public class BattleCompoundInventory: BattleInventory
    {
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

                if (_allWeaponDataSO.ContainsKey(item.Key))
                {
                    ItemDataSO so = _allWeaponDataSO[item.Key];

                    LoadItem(item, etcData, so);
                }
                if (AllDataSO.ContainsKey(item.Key))
                {
                    ItemDataSO so = AllDataSO[item.Key];

                    LoadItem(item, etcData, so);
                }
            }
        }

        protected override void SettingAllDataSO()
        {
            _allWeaponDataSO.Clear();
            AllDataSO = new SerializedDictionary<ItemType, ItemDataSO>();

            foreach (ItemDataSO data in allSO)
            {
                if(data is WeaponItemDataSO weapon)
                {
                    _allWeaponDataSO.Add(data.itemType, weapon);
                }
                else
                {
                    AllDataSO.Add(data.itemType, data);
                }
            }
        }
    }
}