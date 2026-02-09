using System;
using UnityEngine;
using DG.Tweening;

namespace _02Script.Inventory.Item
{
    public class InventoryItemCard : ItemCard
    {
        public static event Action<ItemDataSO,int,int,float> OnMouseCursor;
        public static event Action<ItemCard,RectTransform,int> OnMouseClick;

        [SerializeField] protected float delay = 1f;
        
        private RectTransform rT;

        private void Awake()
        {
            rT = gameObject.GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            MouseExit();
        }

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseCursor?.Invoke(itemData.ReturnDataSO(), itemData.ItemCount(),star,itemHp);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseCursor?.Invoke(null,0,0,0);
        }

        //카드 클릭 (사용할지 묻기)
        public void SelectCard()
        {
            //아이템이 아니면 다 hp 반환
            OnMouseClick?.Invoke(this,rT,itemData.ReturnDataSO().category != ItemCategory.food? itemHp : star);
        }
        public void CancelCard()
        {
            OnMouseClick?.Invoke(null,null,0);
            //클릭 그 표시나게
        }
        #endregion
    }
}