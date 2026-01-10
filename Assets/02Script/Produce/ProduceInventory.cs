using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Produce
{
    public class ProduceInventory : InventoryManager
    {
        [Space(50f)]
        [Header("Produce------------------")]
        [SerializeField] private ProduceBookCard bookPrefab;
        [SerializeField] private ProduceBookSO[] allBookSOs;
        [SerializeField] private ItemCategory[] stuffFilters; //재료 종류
        [SerializeField] private ItemCategory produceFilter; //제작 종류

        private void Start()
        {
            if (itemInventory[produceFilter].childCount <= 0)
            {
                SettingBooks();
            }
        }

        public int ItemCount(ItemDataSO item)
        {
            return _itemDatas[item].ItemCount();
        }

        public ItemCard ItemCardCopy(ItemDataSO soData, Transform parent) //제작대에 카드 복사해 생성
        {
            ItemData data = new ItemData();
            data.NewItem(soData);
            
            ItemCard card = Instantiate(cardPrefab, parent);
            card.NewCard(data);
            
            return card;
        }

        public void CountDistribution(List<ItemCard> items, int maxCount, bool isItem 
            /*true : items(제작대)가 추가, false : 원본(카드)이 추가*/)
        {
            foreach (ItemCard mixtureCard in items)
            {
                ItemCard inventoryItem = _itemCards[_itemDatas[mixtureCard.ReturnData().ReturnDataSO()]][0]; //데이터가 같다는 보장이 없어서

                if (isItem)
                {
                    //수를 키우고
                    mixtureCard.ReturnData().GetItem(maxCount);
                    //수를 줄이고
                    inventoryItem.ReturnData().UseItem(maxCount,true);
                }
                else
                {
                    //수를 키우고
                    inventoryItem.gameObject.SetActive(true);
                    inventoryItem.ReturnData().GetItem(maxCount);
                    //수를 줄이고
                    mixtureCard.ReturnData().UseItem(maxCount,true);
                }
                
                mixtureCard.UpdateCountUI();
                inventoryItem.UpdateCountUI();
            }
        }
        
        public override void AddItem(ItemDataSO item, int count = 1)
        {
            if (stuffFilters.Contains(item.category))
            {
                base.AddItem(item, count);
            }
        }

        private void SettingBooks()
        {
            Transform parent = itemInventory[produceFilter];
            foreach (ProduceBookSO bookSO in allBookSOs)
            {
                ItemData itemData = new ItemData();
                itemData.NewItem(bookSO);
            
                //카드 새 생성
                ProduceBookCard newCard = Instantiate(bookPrefab, parent);
                newCard.gameObject.SetActive(true);
                newCard.NewCard(itemData);
            }
        }
    }
}