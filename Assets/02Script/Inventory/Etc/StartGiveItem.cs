using System.Collections.Generic;
using _02Script.Inventory.Item;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Etc
{
    public class StartGiveItem : GetItem
    {
        [SerializeField] private SerializedDictionary<ItemDataSO, List<int>>  itemData;

        private void Start()
        {
            foreach (KeyValuePair<ItemDataSO, List<int>> item in itemData)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    OnGetItem?.Invoke(item.Key,item.Value[i]);
                }
            }
        }
    }
}