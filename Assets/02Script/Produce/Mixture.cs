using System;
using System.Collections.Generic;
using _02Script.DoTweenUI.Warring;
using _02Script.Farming;
using _02Script.GameEvent;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Obj;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce
{
    public class Mixture: MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] protected float setTimer = 0.7f;
        [Header("Need")]
        [SerializeField] EventLimitWarring eventLimitWarring;
        [SerializeField] protected GameObject errorMassage;
        [SerializeField] protected ProduceLoadInventory produceInventory;
        [SerializeField] protected LoadInventoryManager inventory;
        [SerializeField] protected List<MixtureImageRow> imageRows;
        
        [SerializeField] protected Image means; //도구창
        [SerializeField] protected Image backGround;
        
        [SerializeField] protected Image result;
        [SerializeField] protected TextMeshProUGUI resultCount;
        [SerializeField] protected TextMeshProUGUI resultName;
        [SerializeField] protected TextMeshProUGUI resultExplanation;
        
        protected List<ItemCard> _cards = new List<ItemCard>();
        protected (int maxCount,ItemDataSO data) _itemMax; //비교 & 저장용
        protected ItemDataSO resultData;

        protected float _timer;
        protected bool _isTimer;
        
        //밤 & 이벤트 제한
        protected string _warringText;
        protected bool _isCanUse;
        
        #region EnDi
        protected virtual void OnEnable()
        {
            _isTimer = false;
            _isCanUse = true; 
            ProduceBookCard.OnMouseClick += Setting;
            GameEventManager.OnLockUI += CantUse;
            GameEventManager.OnFarmTemperature += EndEvent;
            Setting(null);
        }
        protected virtual void OnDisable()
        {
            ProduceBookCard.OnMouseClick -= Setting;
            GameEventManager.OnLockUI -= CantUse;
            GameEventManager.OnFarmTemperature -= EndEvent;
        }
        #endregion

        #region Btn
        public virtual void MouseEnter() // 누르는 시간 재기 시작
        {
            _timer = setTimer;
            _isTimer = true;
        }
        public virtual void MouseExit() //누른 시간에 따라 전부 or 하나만 얻을지
        {
            _isTimer = false;
            if (!_isCanUse)
            {
                WarringManager.Warring.ShowWarring(_warringText);
                return;
            }
            GetResult(_timer <= 0? _itemMax.maxCount : 1);
        }
        protected virtual void GetResult(int count) //제작 아이템 얻기 & 재료 버리기
        {
            int use = resultData.category switch
            {
                ItemCategory.food => 1,
                ItemCategory.weapon => 1,
                ItemCategory.armor => 1,
                ItemCategory.machine => 1,
                _=> count,
            };
            count = resultData.category switch
            {
                ItemCategory.food => 5,
                ItemCategory.weapon => 100,
                ItemCategory.armor => 100,
                ItemCategory.machine => 100,
                _=> count,
            };
            foreach (var card in _cards)
            {
                card.ReturnData().UseItem(use,true);
                card.UpdateCountUI();
                
                inventory.ThrowItem(card.ReturnData().ReturnDataSO(),use);
            }
            inventory.AddItem(resultData,count);
            produceInventory.AddItem(resultData,count);
            _itemMax.maxCount -= use;
            
            resultCount.text = _itemMax.maxCount > 0? _itemMax.maxCount.ToString() : "";
            errorMassage.SetActive(_itemMax.maxCount <= 0);
        }
        #endregion

        #region Event or Night
        
        protected virtual void CantUse(DoUIType type)
        {
            (bool,string) i = eventLimitWarring.CantUse(type);
            _warringText = i.Item2;
            _isCanUse = i.Item1;
        }

        protected virtual void EndEvent(TemperatureType _)
        {
            _isCanUse = true;
        }
        #endregion

        protected virtual void Update()
        {
            if (_isTimer)
            {
                _timer -= Time.deltaTime;
            }
        }
        
        protected virtual void ReturnItem()//다시 돌려주기
        {
            produceInventory.CountDistribution(_cards, _itemMax.maxCount,false);
        }

        protected virtual void Setting(ProduceBookSO  bookData) //제작대? 조합대? 세팅 하기
        {
            errorMassage.SetActive(false);
            ReturnItem();
            _itemMax = (Int32.MaxValue, null);
            //null이면 정리 아니면 세팅
            means.sprite = bookData != null? bookData.means.itemImage : null;
            backGround.sprite = bookData != null? bookData.means.background : null;
            
            resultData = bookData != null? bookData.result : null;
            result.sprite = bookData != null? resultData.itemImage : null;
            resultName.text = bookData != null? resultData.itemName : "";
            resultExplanation.text = bookData != null? resultData.itemExplanation : "";
            resultCount.text = "";
            //비워 (조합대 복제본들)
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
                    
                    if(bookData == null || bookData.itemRows[i].items[j] == null) continue; //제작대에 아이템 채워야 하는 일 발생
                    
                    ItemDataSO curItem = bookData.itemRows[i].items[j];
                    imageRows[i].items[j].sprite = curItem.itemImage;
                    imageRows[i].items[j].color = new  Color(1, 1, 1, 100/255f);
                    
                    //중복으로 존재 해야 하는지 확인
                    if (duplication.ContainsKey(curItem))
                    {
                        duplication[curItem]++;

                        if (_itemMax.data == curItem) //중복 만큼 나눠서 max 갱신
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

            resultCount.text = _itemMax.maxCount.ToString();
            if (_itemMax.maxCount <= 0 || _itemMax.maxCount > 9000)
            {
                resultCount.text = "";
                errorMassage.SetActive(true);
                foreach (var card in _cards)
                {
                    card.UpdateCountUI();
                }
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