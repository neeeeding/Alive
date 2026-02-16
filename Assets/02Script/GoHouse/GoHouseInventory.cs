using System.Collections.Generic;
using _02Script.GoHouse.SO;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;

namespace _02Script.GoHouse
{
    public class GoHouseInventory : LoadInventoryManager
    {
        private List<(ItemDataSO, int)> _wantGet = new List<(ItemDataSO, int)>();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            BossItemSO.OnGetItem += GetBossItem;
            HouseSO.OnPortalEnter += AllWantGet;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            BossItemSO.OnGetItem -= GetBossItem;
        }

        private void GetBossItem(ItemDataSO item, int count)
        {
            _wantGet.Add((item, count));
            HouseSO.OnPortalEnter -= AllWantGet;
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