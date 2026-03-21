using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Etc
{
    public class WeaponArmorStartGiveItem : StartGiveItem
    {
        public static Action<ItemDataSO,int, List<BuffType>> OnGetBuff;
        
        [SerializeField] private SerializedDictionary<ItemDataSO, List<List<BuffType>>>  buffType;

        protected override void Set()
        {
            foreach (KeyValuePair<ItemDataSO, List<List<BuffType>>> buff in buffType)
            {
                for (int i = 0; i < buff.Value.Count; i++)
                {
                    OnGetBuff?.Invoke(buff.Key,itemData[buff.Key][i],buff.Value[i]);
                }
            }
            gameObject.SetActive(false);
        }
    }
}