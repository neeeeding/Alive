using System.Collections.Generic;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Collect.Item
{
    public class CollectItemManager : MonoBehaviour
    {
        [SerializeField] private CollectItem itemPrefab;
        [SerializeField] private Transform parent;
        private static List<CollectItem> _items = new List<CollectItem>();

        public void SettingItem(ItemDataSO data, int count, Vector3 position)
        {
            if (_items.Count <= 0)
            {
                CollectItem i = Instantiate(itemPrefab, parent);
                i.gameObject.SetActive(false);
                _items.Add(i);
            }
            
            CollectItem item = _items[0];
            item.transform.position = position;
            item.SetItem(data,count);
            item.gameObject.SetActive(true);
            _items.RemoveAt(0);
        }

        public static void ItemBackList(CollectItem item)
        {
            _items.Add(item);
            item.gameObject.SetActive(false);
        }
        
        public void SetSpawnList(List<(ItemDataSO, Vector3, int)> spawnList)
        {
            foreach ((ItemDataSO item, Vector3 pos, int count) item in spawnList)
            {
                SettingItem(item.item, item.count, item.pos);
            }
        }
    }
}