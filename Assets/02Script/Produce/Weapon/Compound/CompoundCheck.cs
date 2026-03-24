using System;
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
        }

        private void SetStuff(CompoundSelectStuffCard card)
        {
            _stuff = card.ReturnData().ReturnDataSO() as StuffItemDataSO;
        }

        private void SetWeaponArmor(CompoundSelectWeaponArmorCard card,WeaponArmorSaveData data)
        {
            _weaponArmorData = data;
        }

        public void Compound()
        {
            if (_stuff == null || _weaponArmorData == null) return;
            
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