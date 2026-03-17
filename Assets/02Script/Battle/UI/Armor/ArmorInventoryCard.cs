using System;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Armor
{
    public class ArmorInventoryCard: ItemCard
    {
        public static event Action<ArmorInventoryCard,EntityName> OnMouseClick;
        public static Action<ArmorItemDataSO,Vector3> OnMouseEnter; //정보, 현재 남은 시간 

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected Slider damageSlider;
        [SerializeField] protected float delay = 0.2f;

        private EntityName _armorEntity;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke((itemData.ReturnDataSO() as ArmorItemDataSO),gameObject.transform.position);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke(null,Vector3.zero);
        }

        public void MouseClick()
        {
            OnMouseClick?.Invoke(this,_armorEntity);
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
            _armorEntity = entityName;
        }

        public override void UpdateCountUI()
        {
            damageSlider.value = itemHp / 100;
            countUI.text = $"{itemHp} / 100";
        }
    }
}