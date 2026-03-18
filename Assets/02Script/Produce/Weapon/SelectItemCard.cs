using System;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.UI.Etc;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon
{
    public class SelectItemCard : WindowMove
    {
        public static event Action<ItemDataSO, RectTransform> OnMouseCursor; //설명
        public static Action<SelectProduceType> OnMouseUp;
        
        public static SelectItemCard curSelectItem;
        
        [SerializeField]private Image image;
        [SerializeField]private GameObject rockImage;
        
        public SelectProduceType Select{get => _select;}
        public StuffItemDataSO StuffItemData{get => _itemData;}
        
        private SelectProduceType _select;
        private Transform _dragTransform;
        private Transform _baseTransform;
        private RectTransform _meRT;
        private StuffItemDataSO _itemData;
        private InventoryManager _inventory;
        private int _myIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            SettingCard();
        }

        private void Awake()
        {
            _select = SelectProduceType.None;
            _meRT = gameObject.GetComponent<RectTransform>();
        }

        public ItemDataSO GetCurProduce(bool isBase)
        {
            if (isBase) return _itemData.baseData;
            if(_select == SelectProduceType.None) return null;
            
            if(_select == SelectProduceType.Armor) return _itemData.armor;
            return _itemData.weapon;
        }

        public void SetProduce(SelectProduceType produce)
        {
            _select = produce;
        }

        public void SetCard(int index,StuffItemDataSO data ,Transform baseT, Transform dragT,InventoryManager inventory)
        {
            _myIndex = index;
            _itemData = data;
            _baseTransform = baseT;
            _dragTransform = dragT;
            _inventory = inventory;
            
            image.sprite = _itemData.itemImage;
            SettingCard();
        }

        private void SettingCard()
        {
            rockImage.SetActive(!_inventory.FindItem(_itemData.baseData));
        }

        #region Mouse
        public override void MouseClick()
        {
            if(!rockImage.activeSelf) return;
            
            moveObj.transform.SetParent(_dragTransform);
            base.MouseClick();
            image.raycastTarget = false;
            curSelectItem = this;
        }

        public override void MouseCancel()
        {
            if(!rockImage.activeSelf) return;
            
            OnMouseUp?.Invoke(_select);
            base.MouseCancel();
            image.raycastTarget = true;
            
            moveObj.transform.SetParent(_baseTransform);
            moveObj.transform.SetSiblingIndex(_myIndex);
        }

        public void ShowExplanation()
        {
            OnMouseCursor?.Invoke(_itemData,_meRT);
        }

        public void HideExplanation()
        {
            OnMouseCursor?.Invoke(null,null);
        }
        #endregion
    }
    
    public enum SelectProduceType
    {
        None,
        Weapon,
        Armor,
    }
}