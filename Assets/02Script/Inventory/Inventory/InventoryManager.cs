using System.Collections.Generic;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected ItemCard cardPrefab;
        [SerializeField] protected Transform itemCardParent;
        [SerializeField] protected Transform etcCardParent;
        
        protected Dictionary<ItemDataSO, ItemData> _itemDatas = new Dictionary<ItemDataSO, ItemData>();
        protected Dictionary<ItemData, ItemCard> _itemCards = new Dictionary<ItemData, ItemCard>();

        #region EnDi
        protected virtual void OnEnable()
        {
            InGameItem.OnGetItem += GetItem;
        }

        protected virtual void OnDisable()
        {
            InGameItem.OnGetItem -= GetItem;
        }
        #endregion
        public virtual void AddItem(ItemDataSO item, int count = 1)
        {
            if (!_itemDatas.ContainsKey(item))
            {
                NewCard(item);
            }

            ItemData data = _itemDatas[item];
            data.GetItem(count);
            _itemCards[data].UpdateCountUI();
        }

        public virtual void UseItem(ItemDataSO item, int count = 1)
        {
            if (_itemDatas.ContainsKey(item))
            {
                ItemData data = _itemDatas[item];
                data.UseItem(count);
                _itemCards[data].UpdateCountUI();
            }
        }

        protected virtual void NewCard(ItemDataSO item)
        {
            //data 새 생성
            ItemData itemData = new ItemData();
            itemData.NewItem(item);
            _itemDatas.Add(item, itemData);
            
            Transform parent = item.isItem? itemCardParent : etcCardParent;
            
            //카드 새 생성
            ItemCard newCard = Instantiate(cardPrefab, parent);
            newCard.gameObject.SetActive(true);
            newCard.NewCard(itemData);
            
            _itemCards.Add(itemData, newCard);
        }

        protected virtual void GetItem(ItemDataSO item)
        {
            AddItem(item);
        }
    }
}