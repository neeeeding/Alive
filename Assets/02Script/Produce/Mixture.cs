using System;
using System.Collections.Generic;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce
{
    public class Mixture : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float setTimer = 0.7f;
        [Header("Need")]
        [SerializeField] private GameObject errorMassage;
        [SerializeField] private ProduceInventory produceInventory;
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private List<MixtureImageRow> imageRows;
        [SerializeField] private Image result;
        [SerializeField] private TextMeshProUGUI resultName;
        [SerializeField] private TextMeshProUGUI resultExplanation;
        
        private List<ItemCard> _cards = new List<ItemCard>();
        private (int maxCount,ItemDataSO data) _itemMax;
        private ItemDataSO resultData;

        private float _Timer;
        private bool isTimer;
        
        #region EnDi
        private void OnEnable()
        {
            errorMassage.SetActive(false);
            isTimer = false;
            ProduceBookCard.OnMouseClick += Setting;
            Setting(null);
        }
        private void OnDisable()
        {
            ProduceBookCard.OnMouseClick -= Setting;
            //ReturnItem();
        }
        #endregion

        #region Btn
        public void MouseEnter()
        {
            _Timer = setTimer;
            isTimer = true;
        }
        public void MouseExit()
        {
            GetResult(_Timer <= 0? _itemMax.maxCount : 1);
            errorMassage.SetActive(_Timer <= 0);
        }
        private void GetResult(int count) //제작 아이템 얻기
        {
            produceInventory.CopyCardDecrease(_cards, count);
            foreach (var card in _cards)
            {
                inventory.UseItem(card.ReturnData().ReturnDataSO(),count);
            }
            inventory.AddItem(resultData,count);
            produceInventory.AddItem(resultData,count);
            _itemMax.maxCount -= count;
        }
        #endregion
        
        private void ReturnItem()//다시 돌려주기
        {
            produceInventory.CountDistribution(_cards, _itemMax.maxCount,false);
        }

        private void Update()
        {
            if (isTimer)
            {
                _Timer -= Time.deltaTime;
            }
        }

        private void Setting(ProduceBookSO  bookData) //제작대? 조합대? 세팅 하기
        {
            
            ReturnItem();
            resultData = bookData != null? bookData.result : null;
            result.sprite = bookData != null? resultData.itemImage : null;
            resultName.text = bookData != null? resultData.itemName : "";
            resultExplanation.text = bookData != null? resultData.itemExplanation : "";

            _itemMax = (Int32.MaxValue, null);
            
            for (int i = 0; i < _cards.Count; i++)
            {
                GameObject obj =  _cards[i].gameObject;
                Destroy(obj);
            }
            
            _cards.Clear();
            
            Dictionary<ItemDataSO, int> duplication =  new Dictionary<ItemDataSO, int>();
            
            for (int i = 0; i < imageRows.Count; i++)
            {
                for (int j = 0; j < imageRows[i].items.Count; j++)
                {
                    imageRows[i].items[j].sprite = null;
                    imageRows[i].items[j].color = new  Color(1, 1, 1, 0);
                    
                    if(bookData == null || bookData.itemRows[i].items[j] == null) continue;
                    
                    ItemDataSO curItem = bookData.itemRows[i].items[j];
                    
                    imageRows[i].items[j].sprite = curItem.itemImage; //이미지
                    imageRows[i].items[j].color = new  Color(1, 1, 1, 100/255f);
                    
                    //중복 확인
                    if (duplication.ContainsKey(curItem))
                    {
                        duplication[curItem]++;

                        if (_itemMax.data == curItem) //갱신
                        {
                            _itemMax.maxCount *= duplication[curItem] - 1;
                            _itemMax.maxCount /= duplication[curItem];
                        }
                    }
                    else
                    {
                        duplication.Add(curItem, 1);
                    }
                    
                    if (_itemMax.maxCount >= produceInventory.ItemCount(curItem)) //max 갱신
                    {
                        _itemMax =  (produceInventory.ItemCount(curItem),curItem);
                    }
                    
                    //카드 복제본
                    _cards.Add(produceInventory.ItemCardCopy(curItem, imageRows[i].items[j].transform.parent));
                }
            }

            if (_itemMax.maxCount <= 0)
            {
                errorMassage.SetActive(true);
            }
            
            if(bookData == null) return;
            
            produceInventory.CountDistribution(_cards, _itemMax.maxCount,true);
        }
    }
    
    [Serializable]
    public class MixtureImageRow
    {
        public List<Image> items;
    }
}