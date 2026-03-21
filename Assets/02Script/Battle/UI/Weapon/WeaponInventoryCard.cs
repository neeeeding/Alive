using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponInventoryCard : ItemCard
    {
        public static event Action<WeaponInventoryCard,List<BuffSO>,EntityName> OnMouseClick;
        public static Action<WeaponItemDataSO,List<BuffSO>,Vector3> OnMouseEnter; //정보, 현재 남은 시간 

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected Slider damageSlider;
        [SerializeField] protected float delay = 0.2f;

        private EntityName _weaponEntity;
        private List<BuffSO> _buffs;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke((itemData.ReturnDataSO() as WeaponItemDataSO),_buffs,gameObject.transform.position);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one , delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke(null,null,Vector3.zero);
        }

        public void MouseClick()
        {
            OnMouseClick?.Invoke(this,_buffs,_weaponEntity);
        }
        #endregion

        private void OnDisable()
        {
            MouseExit();
        }

        #region NewCard
        public void NewCard(BuffFind buffFind,ItemData itemData,int setStar = 5, int setItemHp = 100, WeaponArmorSaveData data = null)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp, data); //부산물
            
            if(data == null) return;
            _buffs.Clear();
            foreach (BuffType buff in data.buffTypes)
            {
                _buffs.Add(buffFind.GetBuff(buff));
            }
        }

        public override void NewCard(ItemData itemData,int setStar = 5, int setItemHp = 100, WeaponArmorSaveData data = null)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp, data); //부산물
        }
        #endregion

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