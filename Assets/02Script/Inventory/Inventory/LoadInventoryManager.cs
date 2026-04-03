using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Produce.Weapon;
using _02Script.SaveData;
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
            HouseManager.OnStart += LoadItem;
    
            if(HouseManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LoadCard.OnLoad -= LoadItem;
            HouseManager.OnStart -= LoadItem;
        }

        protected virtual void LoadItem() //불러오기
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = HouseManager.Instance.PlayerStat.items.ToDictionary();
            Dictionary<ItemType, List<WeaponArmorSaveData>> etcData = HouseManager.Instance.PlayerStat.weaponArmor.ToDictionary();
            LoadItem(save, etcData);
        }

        protected virtual void LoadItem(Dictionary<ItemType, List<float>> save, Dictionary<ItemType, List<WeaponArmorSaveData>> etcData)
        {
            foreach (var cardList in ItemCards.Values)
            foreach (var card in cardList)
                if (card != null) Destroy(card.gameObject);
            ItemCards.Clear();
            ItemDatas.Clear();

            foreach (KeyValuePair<ItemType, List<float>> item in save.ToList())
            {
                if (item.Key == ItemType.notting) continue;
                if (AllDataSO == null)
                {
                    SettingAllDataSO();
                }
                
                if (!AllDataSO.ContainsKey(item.Key)) continue;

                ItemDataSO so = AllDataSO[item.Key];

                LoadItem(item, etcData, so);
            }
        }
        protected virtual void LoadItem(KeyValuePair<ItemType, List<float>> item, Dictionary<ItemType, List<WeaponArmorSaveData>> etcData,ItemDataSO so)
        {
            switch (so.category)
            {
                case ItemCategory.food:
                case ItemCategory.weapon:
                case ItemCategory.armor:
                case ItemCategory.machine:
                    int count = item.Value.Count;
                    for (int i = 1; i < count; i++)
                    {
                        WeaponArmorSaveData saveData = null;
                        if(etcData.ContainsKey(item.Key))
                            saveData = etcData[item.Key][i-1];
                            
                        NewCard(so, ItemDatas.ContainsKey(so), (int)item.Value[i], (int)item.Value[i],saveData);
                        if (!ItemDatas.ContainsKey(so))
                        {
                            continue;
                        }
                        ItemData data = ItemDatas[so];
                        data.AddCountOnly();
                        ItemCards[data][ItemCards[data].Count - 1].UpdateCountUI();
                    }
                    break;

                default:
                {
                    float val = item.Value[0];
                        
                    NewCard(so, false, 0, 0);
                    if (ItemDatas.ContainsKey(so))
                    {
                        ItemData data = ItemDatas[so];
                        data.SetCountOnly((int)val);
                        ItemCards[data][ItemCards[data].Count - 1].UpdateCountUI();
                    }
                }
                    break;
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