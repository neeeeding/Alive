using System;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponInventoryCard : ItemCard
    {
        public static event Action<WeaponInventoryCard,EntityName> OnMouseClick;
        public static Action<WeaponItemDataSO,Vector3> OnMouseEnter; //정보, 현재 남은 시간 

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected Slider damageSlider;
        [SerializeField] protected float delay = 0.2f;

        private EntityName _weaponEntity;
        private BuffSO[] _buffs;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke((itemData.ReturnDataSO() as WeaponItemDataSO),gameObject.transform.position);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke(null,Vector3.zero);
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

        public void NewCard(ItemData itemData,BuffSO[] buffs,int setStar = 5, int setItemHp = 100)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            _buffs = buffs;
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