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
        public static event Action<WeaponInventoryCard, List<BuffSO>, EntityName, WeaponArmorSaveData> OnMouseClick;
        public static Action<WeaponItemDataSO, WeaponArmorSaveData, List<BuffSO>, Vector3> OnMouseEnter;

        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected Slider damageSlider;
        [SerializeField] protected float delay = 0.2f;

        private EntityName _weaponEntity;
        private List<BuffSO> _buffs;

        public EntityName GetEntity() => _weaponEntity;

        #region Btn
        public void MouseEnter()
        {
            gameObject.transform.DOScale(Vector3.one * 1.15f, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke((itemData.ReturnDataSO() as WeaponItemDataSO), weaponArmorBuff, _buffs, gameObject.transform.position);
        }        
        public void MouseExit()
        {
            gameObject.transform.DOScale(Vector3.one, delay).SetEase(Ease.InOutBack).SetUpdate(true);
            OnMouseEnter?.Invoke(null, null, null, Vector3.zero);
        }

        public void MouseClick()
        {
            OnMouseClick?.Invoke(this, _buffs, _weaponEntity, weaponArmorBuff);
        }
        #endregion

        private void OnDisable()
        {
            MouseExit();
        }

        #region NewCard
        public void NewCard(BuffFind buffFind, ItemData itemData, int setStar = 5, int setItemHp = 100, WeaponArmorSaveData data = null)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp, data);
            
            if (data == null) return;
            
            if (_buffs == null)
            {
                _buffs = new List<BuffSO>();
            }
            _buffs.Clear();
            foreach (BuffType buff in data.buffTypes)
            {
                _buffs.Add(buffFind.GetBuff(buff));
            }
            UpdateCountUI();
        }

        public override void NewCard(ItemData itemData, int setStar = 5, int setItemHp = 100, WeaponArmorSaveData data = null)
        {
            nameText.text = itemData.ReturnDataSO().itemName;
            base.NewCard(itemData, setStar, setItemHp, data);
            UpdateCountUI();
        }
        #endregion

        public void Set(EntityName entityName)
        {
            _weaponEntity = entityName;
        }

        // [수정] 슬라이더 및 내구도 텍스트 실시간 100% 반영
        public override void UpdateCountUI()
        {
            if (damageSlider != null)
            {
                if (damageSlider.maxValue > 1.5f)
                {
                    damageSlider.value = itemHp;
                }
                else
                {
                    damageSlider.value = itemHp / 100f;
                }
            }
            if (countUI != null)
            {
                countUI.text = $"{Mathf.CeilToInt(itemHp)} / 100";
            }
        }
    }
}
