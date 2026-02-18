using System.Collections.Generic;
using System.Linq;
using _02Script.Etc;
using _02Script.GoHouse.SO;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.UI.Save;

namespace _02Script.GoHouse.Etc
{
    public class GoHouseInventory : LoadInventoryManager
    {
        private List<(ItemDataSO, int)> _wantGet = new List<(ItemDataSO, int)>();
        
        protected override void OnEnable()
        {
            LoadCard.OnLoad += LoadItem;
            
            if(GoHouseSaveManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
            
            BossItemSO.OnGetItem += GetBossItem;
            HouseSO.OnSuccess += AllWantGet;
            GoHouseSaveManager.OnSaveItem += AllItemGet;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            BossItemSO.OnGetItem -= GetBossItem;
            HouseSO.OnSuccess -= AllWantGet;
            GoHouseSaveManager.OnSaveItem -= AllItemGet;
        }

        protected override void LoadItem() //불러오기
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = GoHouseSaveManager.Instance.PlayerStat.items.ToDictionary();

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

        private void AllItemGet(SaveDictionary<ItemType, List<float>> allItems)
        {
            foreach (KeyValuePair<ItemType, List<float>> items in allItems.ToDictionary())
            {
                foreach (float item in items.Value)
                {
                    AddItem(AllDataSO[items.Key], (int)item);
                }
            }
        }

        private void GetBossItem(ItemDataSO item, int count)
        {
            _wantGet.Add((item, count));
            HouseSO.OnSuccess -= AllWantGet;
        }

        private void AllWantGet(string s,BlockActionSO b)
        {
            foreach ((ItemDataSO data, int count) get in _wantGet)
            {
                AddItem(get.data, get.count);
            }
        }
    }
}