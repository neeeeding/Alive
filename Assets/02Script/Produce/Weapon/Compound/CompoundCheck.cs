using System;
using _02Script.DoTweenUI.Warring;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundCheck : MonoBehaviour
    {
        public static Action<StuffItemDataSO,WeaponArmorSaveData,CompoundSelectWeaponArmorCard> OnCompound;
        
        private StuffItemDataSO _stuff;
        private CompoundSelectWeaponArmorCard _weaponArmor;
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
            _weaponArmor = card;
            _weaponArmorData = data;
        }

        public void Compound()
        {
            if (_stuff == null || _weaponArmor == null)
            {
                WarringManager.Warring.ShowWarring("재료 혹은 무기나 갑옷을 선택해주세요.");
                return;
            }
            
            OnCompound?.Invoke(_stuff,_weaponArmorData,_weaponArmor);
            ResetSelect();
        }

        private void ResetSelect()
        {
            _stuff = null;
            _weaponArmor = null;
        }
        
    }
}