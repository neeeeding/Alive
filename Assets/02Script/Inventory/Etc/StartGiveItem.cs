using _02Script.Inventory.Item;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Etc
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