using System;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI
{
    public class WeaponInventoryCard : ItemCard
    {
        public static event Action<WeaponInventoryCard,EntityName> OnMouseClick;

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected Slider damageSlider;
        [SerializeField] protected float delay = 0.2f;

        private EntityName _weaponEntity;

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
            OnMouseClick?.Invoke(this,_weaponEntity);
        }
        #endregion

        private void OnDisable()
        {
            MouseExit();
        }

        public override void NewCard(ItemData itemData, int setStar = 5, int setItemHp = 100)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp); //부산물
        }

        public void Set(EntityName entityName)
        {
            _weaponEntity = entityName;
        }

        public override void UpdateCountUI()
        {
            damageSlider.value = itemHp / 100;
            countUI.text = $"{itemHp} / 100";
        }
    }
}