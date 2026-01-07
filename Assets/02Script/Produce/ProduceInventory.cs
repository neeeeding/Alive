using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce
{
    public class ProduceInventory : InventoryManager
    {
        [SerializeField] private ItemType[] filters;

        public int ItemCount(ItemDataSO item)
        {
            return _itemDatas[item].ItemCount();
        }

        public ItemCard ItemCardCopy(ItemDataSO soData, Transform parent)
        {
            ItemData data = new ItemData();
            data.NewItem(soData);
            
            ItemCard card = Instantiate(cardPrefab, parent);
            card.NewCard(data);
            
            return card;
        }

        public void CopyCardDecrease(List<ItemCard> items,int count)
        {
            foreach (var item in items)
            {
                item.ReturnData().UseItem(count,true);
                
                item.UpdateCountUI();
            }
        }

        public void CountDistribution(List<ItemCard> items, int maxCount, bool isItem 
            /*true : items가 추가, false : 원본이 추가*/)
        {
            foreach (var mixtureCard in items)
            {
                ItemCard inventoryItem = _itemCards[_itemDatas[mixtureCard.ReturnData().ReturnDataSO()]]; //데이터가 같다는 보장이 없어서

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
                    inventoryItem.ReturnData().GetItem(maxCount);
                    inventoryItem.gameObject.SetActive(true);
                    //수를 줄이고
                    mixtureCard.ReturnData().UseItem(maxCount,true);
                }
                
                mixtureCard.UpdateCountUI();
                inventoryItem.UpdateCountUI();
            }
        }
        
        protected void GetItem(ItemDataSO item)
        {
            if (filters.Contains(item.itemType))
            {
                base.GetItem(item);
            }
        }
    }
}