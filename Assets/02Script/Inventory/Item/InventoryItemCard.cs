using System;
using UnityEngine;
using DG.Tweening;

namespace JYE._01Script.Inventory.Item
{
    public class InventoryItemCard : ItemCard
    {
        public static event Action<ItemDataSO,int> OnMouseCursor;
        public static event Action<ItemCard,RectTransform> OnMouseClick;

        [SerializeField] private float delay = 1f;
        
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
            OnMouseCursor?.Invoke(itemData.ReturnData(), itemData.ItemCount());
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseCursor?.Invoke(null,0);
        }

        //카드 클릭 (사용할지 묻기)
        public void SelectCard()
        {
            OnMouseClick?.Invoke(this,rT);
        }
        #endregion
    }
}