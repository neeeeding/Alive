using System.Collections.Generic;
using _02Script.Inventory.Item;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private ItemCard cardPrefab;
        [SerializeField] SerializedDictionary<ItemCategory, Transform> itemInventory;
        
        private Dictionary<ItemDataSO, ItemData> _itemDatas = new Dictionary<ItemDataSO, ItemData>();
        private Dictionary<ItemData, ItemCard> _itemCards = new Dictionary<ItemData, ItemCard>();

        #region EnDi
        private void OnEnable()
        {
            InGameItem.OnGetItem += GetItem;
        }

        private void OnDisable()
        {
            InGameItem.OnGetItem -= GetItem;
        }
        #endregion
        public void AddItem(ItemDataSO item, int count = 1)
        {
            if (!_itemDatas.ContainsKey(item))
            {
                NewCard(item);
            }

            ItemData data = _itemDatas[item];
            data.GetItem(count);
            _itemCards[data].UpdateCountUI();
        }

        public void UseItem(ItemDataSO item, int count = 1)
        {
            Use(item, false, count);
        }

        public void ThrowItem(ItemDataSO item, int count = 1)
        {
            Use(item, true, count);
        }

        private void Use(ItemDataSO item, bool isThrow,int count = 1)
        {
            if (_itemDatas.ContainsKey(item))
            {
                ItemData data = _itemDatas[item];
                data.UseItem(count, isThrow);
                _itemCards[data].UpdateCountUI();
            }
        }

        private void NewCard(ItemDataSO item)
        {
            //data 새 생성
            ItemData itemData = new ItemData();
            itemData.NewItem(item);
            _itemDatas.Add(item, itemData);
            
            Transform parent = itemInventory[item.category];
            
            //카드 새 생성
            ItemCard newCard = Instantiate(cardPrefab, parent);
            newCard.gameObject.SetActive(true);
            newCard.NewCard(itemData);
            
            _itemCards.Add(itemData, newCard);
        }

        private void GetItem(ItemDataSO item)
        {
            AddItem(item);
        }
    }
}