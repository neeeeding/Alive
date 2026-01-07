using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.UI.Save;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using ItemCard = _02Script.Inventory.Item.ItemCard;

namespace _02Script.Inventory.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected ItemCard cardPrefab;
        [SerializeField] protected SerializedDictionary<ItemCategory, Transform> itemInventory;
        [SerializeField] protected SerializedDictionary<ItemType, ItemDataSO> allDataSO;
        
        protected Dictionary<ItemDataSO, ItemData> _itemDatas = new Dictionary<ItemDataSO, ItemData>();
        protected Dictionary<ItemData, ItemCard> _itemCards = new Dictionary<ItemData, ItemCard>();
        
        //hold에 대해
        [SerializeField]private ItemHold realItem; //들리게 될 아이템(위치)

        #region EnDi
        private void OnEnable()
        {
            InGameItem.OnGetItem += GetItem;
            LoadCard.OnLoad += LoadItem;
            
            if(GameManager.Instance.isStart)
            {
                LoadItem();
            }
        }

        private void OnDisable()
        {
            InGameItem.OnGetItem -= GetItem;
            LoadCard.OnLoad -= LoadItem;
        }
        #endregion

        private void LoadItem() //불러오기
        {
            Dictionary<ItemType, int> save = GameManager.Instance.PlayerStat.items.ToDictionary();

            foreach (KeyValuePair<ItemType, ItemDataSO> item in allDataSO.ToList())
            {
                ThrowItem(item.Value,9999999);
            }

            foreach (KeyValuePair<ItemType, int> item in save.ToList())
            {
                AddItem(allDataSO[item.Key], item.Value);
            }
        }
        
        public void HoldItem(ItemData item, int count = 1) //들기
        {
            realItem.Setting(item, count);
        }

        public void UseItem(ItemDataSO item, int count = 1) //사용
        {
            LessItem(item, false, count);
        }

        public void ThrowItem(ItemDataSO item, int count = 1) //버리기
        {
            LessItem(item, true, count);
        }

        private void LessItem(ItemDataSO item, bool isThrow,int count = 1) //어쨌든 아이템 감소
        {
            if (_itemDatas.ContainsKey(item))
            {
                ItemData data = _itemDatas[item];
                data.UseItem(count, isThrow);
                _itemCards[data].UpdateCountUI();
                
                realItem.CheckLessItem();
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

        protected void GetItem(ItemDataSO item)
        {
            AddItem(item);
        }
        public void AddItem(ItemDataSO item, int count = 1)
        {
            if (!_itemDatas.ContainsKey(item))
            {
                NewCard(item);
            }

            ItemData data = _itemDatas[item];
            data.GetItem(count);
            _itemCards[data].gameObject.SetActive(true);
            _itemCards[data].UpdateCountUI();
        }
    }
}