using System;
using _02Script.Inventory.Item;
using DG.Tweening;
using UnityEngine;

namespace _02Script.Collect
{
    public class CollectInventoryCard: ItemCard
    {
        public static event Action<CollectInventoryCard,ItemDataSO,int,int> OnMouseClick;
        
        [SerializeField] protected float delay = 0.2f;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
        }
        #endregion

        public override void NewCard(ItemData itemData, int setStar = 5, int setItemHp = 100)
        {
            countUI.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp); //부산물
        }

        public override void UpdateCountUI()
        {
        }
    }
}