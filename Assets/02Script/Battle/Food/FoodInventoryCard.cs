using System;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Food
{
    public class FoodInventoryCard: ItemCard
    {
        public static event Action<FoodInventoryCard> OnMouseClick;

        [Header("Setting")]
        [SerializeField] private Color baseColor = Color.green;
        [SerializeField] private Color changeColor = Color.orange;
        [SerializeField] protected float delay = 0.2f;
        [Header("Need")]
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI addStatText;
        [SerializeField] private Image _myImage;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
        }

        public void MouseClick()
        {
            OnMouseClick?.Invoke(this);
        }
        #endregion

        #region EnDi
        protected override void OnEnable()
        {
            base.OnEnable();
            FoodInventoryCard.OnMouseClick += ChangeColor;
            FoodCheck.OnFood += Eat;
        }

        private void OnDisable()
        {
            MouseExit();
            FoodInventoryCard.OnMouseClick -= ChangeColor;
            FoodCheck.OnFood -= Eat;
            ChangeColor(null);
        }
        #endregion

        public override void NewCard(ItemData itemData, int setStar = 5, int setItemHp = 100)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            int add = itemData.ReturnDataSO().addStats;
            if (add >= 0)
                addStatText.text = "+";
            addStatText.text += $"{add} {EnumToString.Name(itemData.ReturnDataSO().stats)}";
            base.NewCard(itemData, setStar, setItemHp); //부산물
        }

        public override void UpdateCountUI()
        {
        }

        private void Eat(EntityName name, FoodInventoryCard card)
        {
            ChangeColor(null);
        }

        private void ChangeColor(FoodInventoryCard card)
        {
            _myImage.color = card != this ? baseColor : changeColor;
        }
    }
}