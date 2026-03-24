using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.UI.Save;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class LoadInventoryManager : InventoryManager
    {
        [SerializeField] protected ItemDataSO[] allSO;
        
        protected SerializedDictionary<ItemType, ItemDataSO> AllDataSO;
        protected override void OnEnable()
        {
            base.OnEnable();
            LoadCard.OnLoad += LoadItem;
            
            if(HouseManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LoadCard.OnLoad -= LoadItem;
        }

        protected virtual void LoadItem() //불러오기
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = HouseManager.Instance.PlayerStat.items.ToDictionary();
            LoadItem(save);
        }

        protected virtual void LoadItem(Dictionary<ItemType, List<float>> save)
        {
            foreach (KeyValuePair<ItemType, ItemDataSO> item in AllDataSO.ToList())
            {
                ThrowItem(item.Value,9999999);
            }

            foreach (KeyValuePair<ItemType, List<float>> item in save.ToList())
            {
                foreach (int num in item.Value.ToList())
                {
                    AddItem(AllDataSO[item.Key], num);
                }
            }
        }

        protected virtual void SettingAllDataSO()
        {
            AllDataSO = new SerializedDictionary<ItemType, ItemDataSO>();

            foreach (ItemDataSO data in allSO)
            {
                AllDataSO.Add(data.itemType, data);
            }
        }
    }
}