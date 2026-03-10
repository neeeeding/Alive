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
        
        private List<ItemDataSO> _itme = new List<ItemDataSO>();
        private List<int> _iCount = new List<int>();
        private List<Vector3> _iPos = new List<Vector3>();
        
        
        protected override void ToOrganizeList()
        {
            foreach (CollectItem itemPos in justItemPos)
            {
                setSpawn.Add(itemPos, 0);
            }
            foreach (KeyValuePair<CollectItem, int> spawn in setSpawn)
            {
                _itme.Add(spawn.Key.ItemData);
                _iCount.Add(spawn.Value);
                _iPos.Add(spawn.Key.transform.position);
            }
        }
        
        protected override void GiveList()
        {
            stage.SetItem(_itme,_iCount,_iPos);
            print("ok Item");
            gameObject.SetActive(false);
        }
    }
}