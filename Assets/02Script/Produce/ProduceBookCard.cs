using System;
using _02Script.Inventory.Item;
using _02Script.Produce.Weapon;
using DG.Tweening;
using UnityEngine;

namespace _02Script.Produce
{
    public class ProduceBookCard : ItemCard
    {
        /**이름 뜨기 더 나아가 관련 책 띄어주기??*/
        public static event Action<ItemDataSO, RectTransform> OnMouseCursor; //설명
        public static event Action<ProduceBookSO> OnMouseClick;
        
        [SerializeField] protected float delay = 0.2f;
        
        private ProduceBookSO _bookData;
        private RectTransform rect;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseCursor?.Invoke(itemData.ReturnDataSO(), rect);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseCursor?.Invoke(null, null);
        }
        public void SelectBook()
        {
            if(_bookData == null) return; //책이 아님
            OnMouseClick?.Invoke(_bookData);
        }
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            rect = GetComponent<RectTransform>();
        }

        public override void NewCard(ItemData itemData, int setStar = 5, int setItemHp = 100,WeaponArmorSaveData saveData = null)
        {
            _bookData = itemData.ReturnDataSO() as ProduceBookSO;
            countUI.text = _bookData.result.itemName;
            base.NewCard(itemData, setStar, setItemHp,saveData); //부산물
        }

        public override void UpdateCountUI()
        {
        }
    }
}