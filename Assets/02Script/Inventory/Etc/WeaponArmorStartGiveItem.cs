using System;
using System.Collections.Generic;
using _02Script.Battle;
using _02Script.Inventory.Item;
using _02Script.Produce.Weapon;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Etc
{
    public class WeaponArmorStartGiveItem : StartGiveItem
    {
        public static Action<ItemDataSO,WeaponArmorSaveData,int> OnGetBuff;
        
        [SerializeField] private SerializedDictionary<ItemDataSO, List<WeaponArmorSaveData>>  buffType;

        protected override void Set()
        {
            foreach (KeyValuePair<ItemDataSO, List<WeaponArmorSaveData>>  buff in buffType)
            {
                for (int i = 0; i < buff.Value.Count; i++)
                {
                    if (!BattleSaveManager.Instance.PlayerStat.weaponArmor.ToDictionary().ContainsKey(buff.Key.itemType))
                        BattleSaveManager.Instance.PlayerStat.weaponArmor.Add(buff.Key.itemType, new List<WeaponArmorSaveData>());
                    BattleSaveManager.Instance.PlayerStat.weaponArmor[buff.Key.itemType].Add(buff.Value[i]);
                    OnGetBuff?.Invoke(buff.Key,buff.Value[i],itemData[buff.Key][i]);
                    itemData[buff.Key].RemoveAt(0);
                    if(itemData[buff.Key].Count <= 0)
                        itemData.Remove(buff.Key);
                }
            }
            base.Set();
        }
    }
}