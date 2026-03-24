using System;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundSelectWeaponArmorCard: ItemCard
    {
        public static event Action<CompoundSelectWeaponArmorCard,WeaponArmorSaveData> OnMouseClick;
        public static Action<CompoundSelectWeaponArmorCard,WeaponArmorSaveData,Vector3> OnMouseEnter; //정보, 현재 남은 시간 

        [SerializeField] private Color baseC = Color.white;
        [SerializeField] private Color selectC = Color.green;
        [SerializeField] protected Image colorImage;
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected Slider damageSlider;

        #region Btn
        public void MouseEnter()
        {
            OnMouseEnter?.Invoke(this,weaponArmorBuff,gameObject.transform.position);
        }        
        public void MouseExit()
        {
            OnMouseEnter?.Invoke(null,null,Vector3.zero);
        }

        public void MouseClick()
        {
            OnMouseClick?.Invoke(this,weaponArmorBuff);
        }
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            CompoundSelectWeaponArmorCard.OnMouseClick += ChangeColor;
        }

        private void OnDisable()
        {
            MouseExit();
            CompoundSelectWeaponArmorCard.OnMouseClick -= ChangeColor;
        }
        private void ChangeColor(CompoundSelectWeaponArmorCard card, WeaponArmorSaveData _)
        {
            colorImage.color = card != this ? baseC : selectC;
        }

        #region NewCard
        public override void NewCard(ItemData itemData,int setStar = 5, int setItemHp = 100, WeaponArmorSaveData data = null)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp, data); //부산물
        }
        #endregion

        public override void UpdateCountUI()
        {
            damageSlider.value = itemHp / 100;
            countUI.text = $"{itemHp} / 100";
        }
    }
}