using System;
using System.Collections.Generic;
using _02Script.Battle;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Produce.Weapon;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Inventory.Etc
{
    public class WeaponArmorStartGiveItem : StartGiveItem
    {
        public static Action<ItemDataSO,WeaponArmorSaveData,int> OnGetBuff;
        
        [SerializeField] private SerializedDictionary<ItemDataSO, List<WeaponArmorSaveData>>  buffType;

        protected override void Set()
        {
            if (SceneManager.GetActiveScene().name.Contains("House"))
            {
                if(IsCantGet()) return;
                HouseManager.Instance.PlayerStat.isGetItem = false;
            }
            foreach (KeyValuePair<ItemDataSO, List<WeaponArmorSaveData>>  buff in buffType)
            {
                for (int i = 0; i < buff.Value.Count; i++)
                {
                    if (!SaveManagerCheck.GetCurScenePlayerStat().weaponArmor.ToDictionary().ContainsKey(buff.Key.itemType))
                        SaveManagerCheck.GetCurScenePlayerStat().weaponArmor.Add(buff.Key.itemType, new List<WeaponArmorSaveData>());
                    SaveManagerCheck.GetCurScenePlayerStat().weaponArmor[buff.Key.itemType].Add(buff.Value[i]);
                    //itemData[buff.Key].RemoveAt(0);
                    if(itemData[buff.Key].Count <= 0)
                        itemData.Remove(buff.Key);
                }
            }
            base.Set();
        }
    }
}