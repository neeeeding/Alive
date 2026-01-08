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
            foreach (var item in itemData)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    for (int j = 0; j < item.Value[i]; j++)
                    {
                        OnGetItem?.Invoke(item.Key,j);
                    }
                }
            }
        }
    }
}