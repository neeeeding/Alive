using System;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Collect
{
    public class CollectInventoryCard: ItemCard
    {
        public static event Action<CollectInventoryCard,EntityName,ItemDataSO,float> OnMouseClick;
        
        [SerializeField] private Color baseColor = Color.green;
        [SerializeField] private Color changeColor = Color.orange;
        [SerializeField] protected float delay = 0.2f;
        [SerializeField] private Image _myImage;
        
        private EntityName _character;
        public EntityName Character {get => _character;}

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
            OnMouseClick?.Invoke(this,_character,itemData.ReturnDataSO(), itemData.ReturnDataSO().category != ItemCategory.food? itemHp : star);
        }
        #endregion

        #region EnDi

        protected override void OnEnable()
        {
            base.OnEnable();
            ChangeColor(false);
            CollectInventoryCard.OnMouseClick += Select;
        }

        private void OnDisable()
        {
            CollectInventoryCard.OnMouseClick -= Select;
        }

        #endregion

        public override void NewCard(ItemData itemData, int setStar = 5, int setItemHp = 100,WeaponArmorSaveData saveData = null)
        {
            countUI.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp,saveData); //부산물
        }

        public void SetCharacter(EntityName inventoryCharacter)
        {
            _character = inventoryCharacter;
        }

        public override void UpdateCountUI()
        {
        }
        
        private void Select(CollectInventoryCard card,EntityName name, ItemDataSO so, float count)
        {
            ChangeColor(card == this);
        }

        private void ChangeColor(bool change)
        {
            _myImage.color = !change ? baseColor : changeColor;
        }
    }
}