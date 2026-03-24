using System;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundSelectStuffCard: ItemCard
    {
        public static event Action<CompoundSelectStuffCard> OnMouseClick;
        public static Action<CompoundSelectStuffCard,Vector3> OnMouseEnter; //정보, 현재 남은 시간 

        [SerializeField] private Color baseC = Color.white;
        [SerializeField] private Color selectC = Color.green;
        [SerializeField] protected Image colorImage;
        [SerializeField] protected TextMeshProUGUI nameText;

        #region Btn
        public void MouseEnter()
        {
            OnMouseEnter?.Invoke(this,gameObject.transform.position);
        }        
        public void MouseExit()
        {
            OnMouseEnter?.Invoke(null,Vector3.zero);
        }

        public void MouseClick()
        {
            OnMouseClick?.Invoke(this);
        }
        #endregion
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CompoundSelectStuffCard.OnMouseClick += ChangeColor;
        }

        private void OnDisable()
        {
            MouseExit();
            CompoundSelectStuffCard.OnMouseClick -= ChangeColor;
        }
        private void ChangeColor(CompoundSelectStuffCard card)
        {
            colorImage.color = card != this ? baseC : selectC;
        }
        public override void UpdateCountUI()
        {
            nameText.text = itemData.ReturnDataSO().itemName;
        }
    }
}