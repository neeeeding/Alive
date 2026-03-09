using System.Collections.Generic;
using _02Script.Collect.Item;
using _02Script.Etc;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    public class StageItemSet :SetItemSpawn
    {
        [SerializeField] private BattleStageSO stage;
        
        private Dictionary<ItemDataSO, List<int>> _itemSpawn =new Dictionary<ItemDataSO, List<int>>();
        private Dictionary<ItemDataSO, List<Vector3>> _itemPos =new Dictionary<ItemDataSO, List<Vector3>>();
        
        
        protected override void ToOrganizeList()
        {
            foreach (CollectItem itemPos in justItemPos)
            {
                setSpawn.Add(itemPos, 0);
            }
            foreach (KeyValuePair<CollectItem, int> spawn in setSpawn)
            {
                ItemDataSO item = spawn.Key.ItemData;
                if (!_itemPos.ContainsKey(item))
                {
                    _itemSpawn.Add(item, new List<int>());
                    _itemPos.Add(item, new List<Vector3>());
                }
                
                _itemSpawn[item].Add(spawn.Value);
                _itemPos[item].Add(spawn.Key.transform.position);
            }
        }
        
        protected override void GiveList()
        {
            stage.SetItem(_itemSpawn,_itemPos);
            print("ok Item");
            gameObject.SetActive(false);
        }
    }
}