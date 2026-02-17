using System;
using _02Script.DoTweenUI.Warring;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Obj.Obj;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _02Script.UI.Store
{
    //요리, 기계, 갑옷, 무기는 지불 대상이 될 수 없음
    public class StoreCard : MonoBehaviour
    {
        public static Action<ItemDataSO, int> OnSellItem;
        public static Action<ItemDataSO, int> OnPayItem;
        
        [SerializeField] public GameObject lockImage;
        [Header("Need")]
        [SerializeField] private Image sellImage;
        [SerializeField] private Image payImage;
        [SerializeField] private TextMeshProUGUI sellCountText;
        [SerializeField] private TextMeshProUGUI payCountText;

        private ItemDataSO _sellItem;
        private ItemDataSO _payItem;
        private int _sellCount;
        private int _payCount;
        private bool _isLock;

        public void SetCard(ItemDataSO sellItem, ItemDataSO payItem, int sellC = 1, int payC = 1)
        {
            _sellItem = sellItem;
            _payItem = payItem;
            _sellCount = sellC;
            _payCount = payC;
            
            _isLock = false;
            lockImage.gameObject.SetActive(false);

            sellImage.sprite = sellItem.itemImage;
            payImage.sprite = payItem.itemImage;
            sellCountText.text = sellC + " 개";
            payCountText.text = payC + " 개";
        }

        public void PayCard()
        {
            if(_isLock) return;

            if (HouseManager.Instance.PlayerStat.items[_payItem.itemType][0] < _payCount)
            {
                WarringManager.Warring.ShowWarring("지불할 아이템이 부족합니다.");
                return;
            }
            OnSellItem?.Invoke(_sellItem, _sellCount);
            OnPayItem?.Invoke(_payItem, _payCount);
            
            //대사? (북마크)
            _isLock = true;
            lockImage.gameObject.SetActive(true);
        }
    }
}