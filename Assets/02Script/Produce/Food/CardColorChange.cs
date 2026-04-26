using System;
using System.Collections.Generic;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce.Food
{
    public class CardColorChange : MonoBehaviour
    {
        [SerializeField] private Color baseColor;
        [SerializeField] private Color canColor;
        [SerializeField] private Transform cardParent;
        [SerializeField] private ProduceLoadInventory produceInventory;

        private List<ProduceBookCard> _cardList = new List<ProduceBookCard>();
        
        private void OnEnable()
        {
            GetAllCard();
            ChangeColorCheck();
        }

        private void ChangeColorCheck()
        {
            List<ProduceBookCard> upList = new List<ProduceBookCard>(); // 위로 갱신
            
            foreach (ProduceBookCard card in _cardList) //색상 변경
            {
                bool isCheck = true;
                Dictionary<ItemType,int> itemCount = new Dictionary<ItemType,int>(); //중복 때문에
                foreach (var row in card.GetBookData().itemRows)
                {
                    foreach (ItemDataSO item in row.items)
                    {
                        if(item == null) continue;
                        if (!itemCount.ContainsKey(item.itemType))
                        {
                            itemCount.Add(item.itemType, 1);
                        }
                        itemCount[item.itemType]++;
                        if (produceInventory.ItemCount(item) < itemCount[item.itemType])
                        {
                            isCheck = false;
                        }
                    }
                }
                
                card.ChangeColor(isCheck? canColor: baseColor);
                if (isCheck)
                {
                    upList.Add(card);
                }
            }

            foreach (ProduceBookCard card in upList) //앞으로 갱신
            {
                card.transform.SetSiblingIndex(0);
            }
        }

        private void GetAllCard() //카드들 불러오기
        {
            if(_cardList.Count > 0) return;
            
            for (int i = 0; i < cardParent.childCount; i++)
            {
                _cardList.Add(cardParent.GetChild(i).GetComponent<ProduceBookCard>());
            }
        }
    }
}