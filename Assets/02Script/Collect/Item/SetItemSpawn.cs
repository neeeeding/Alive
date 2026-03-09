using System.Collections.Generic;
using _02Script.Inventory.Item;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Collect.Item
{
    public class SetItemSpawn : MonoBehaviour
    {
        protected List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)> _finishList 
            = new List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)>();

        [SerializeField] protected List<CollectItem> justItemPos = new List<CollectItem>();
        [SerializeField] protected SerializedDictionary<CollectItem, int> setSpawn =new SerializedDictionary<CollectItem, int>();
        [SerializeField] protected CollectItemManager manager;

        private void Awake()
        {
            ToOrganizeList();
            GiveList();
        }

        protected virtual void ToOrganizeList()
        {
            foreach (CollectItem itemPos in justItemPos)
            {
                setSpawn.Add(itemPos, 0);
            }
            foreach (KeyValuePair<CollectItem, int> spawn in setSpawn)
            {
                _finishList.Add((spawn.Key.ItemData, spawn.Key.transform.position, spawn.Value));
            }
        }

        protected virtual void GiveList()
        {
            manager.SetSpawnList(_finishList);
            gameObject.SetActive(false);
        }
    }
}