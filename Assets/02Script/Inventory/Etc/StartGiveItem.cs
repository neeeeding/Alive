using AYellowpaper.SerializedCollections;
using JYE._01Script.Inventory.Item;
using UnityEngine;

namespace JYE._01Script.Inventory.Etc
{
    public class StartGiveItem : GetItem
    {
        [SerializeField] private SerializedDictionary<ItemDataSO,int>  itemData;

        private void Start()
        {
            foreach (var item in itemData)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    OnGetItem?.Invoke(item.Key);
                }
            }
        }
    }
}