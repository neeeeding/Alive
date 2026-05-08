using System;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundSelectStuffCard: MonoBehaviour
    {
        public static event Action<CompoundSelectStuffCard> OnMouseClick;
        public static Action<CompoundSelectStuffCard,Vector3> OnMouseEnter; //정보, 현재 남은 시간 

        [SerializeField] private Color baseC = Color.white;
        [SerializeField] private Color selectC = Color.green;
        [SerializeField] protected Image colorImage;
        [SerializeField] protected Image cardImage;
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField]private GameObject rockImage;
        
        public StuffItemDataSO ItemData{get=>_itemData;}
        
        private StuffItemDataSO _itemData;
        private InventoryManager _inventory;

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
            if(!_inventory.FindItem(_itemData.baseData)) return;
            OnMouseClick?.Invoke(this);
        }
        #endregion
        
        private void OnEnable()
        {
            CompoundSelectStuffCard.OnMouseClick += ChangeColor;
            ChangeColor(null);
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

        public void SetCard(StuffItemDataSO data ,InventoryManager inventory)
        {
            _itemData = data;
            _inventory = inventory;
            
            cardImage.sprite = _itemData.itemImage;
            nameText.text = _itemData.baseData.itemName;
            SettingCard();
        }
        private void SettingCard()
        {
            rockImage.SetActive(!_inventory.FindItem(_itemData.baseData));
        }
    }
}