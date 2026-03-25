using System;
using _02Script.DoTweenUI.Warring;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundCheck : MonoBehaviour
    {
        public static Action<StuffItemDataSO,WeaponArmorSaveData> OnFood;
        
        private StuffItemDataSO _stuff;
        //private WeaponItemDataSO _weaponArmor;
        private WeaponArmorSaveData _weaponArmorData;
        
        private void OnEnable()
        {
            ResetSelect();
            CompoundSelectStuffCard.OnMouseClick += SetStuff;
            CompoundSelectWeaponArmorCard.OnMouseClick += SetWeaponArmor;
        }
        private void OnDisable()
        {
            CompoundSelectStuffCard.OnMouseClick -= SetStuff;
            CompoundSelectWeaponArmorCard.OnMouseClick -= SetWeaponArmor;
            ResetSelect();
        }

        private void SetStuff(CompoundSelectStuffCard card)
        {
            _stuff = card.ItemData;
        }

        private void SetWeaponArmor(CompoundSelectWeaponArmorCard card,WeaponArmorSaveData data)
        {
            _weaponArmorData = data;
        }

        public void Compound()
        {
            if (_stuff == null || _weaponArmorData == null)
            {
                WarringManager.Warring.ShowWarring("재료 혹은 무기나 갑옷을 선택해주세요.");
                return;
            }
            
            OnFood?.Invoke(_stuff,_weaponArmorData);
            ResetSelect();
        }

        private void ResetSelect()
        {
            _stuff = null;
            _weaponArmorData = null;
        }
        
    }
}