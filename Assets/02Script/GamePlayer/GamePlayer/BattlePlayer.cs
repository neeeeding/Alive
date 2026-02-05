using _02Script.Battle.Entity;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.GamePlayer.GamePlayer
{
    public class BattlePlayer : BattleMonster
    {
        [SerializeField] private SpriteRenderer weaponRenderer;
        [SerializeField] private GameObject haveWeapon;
        
        private void OnEnable()
        {
            BattleCharacter.OnWeapon += SetWeapon;
        }

        protected override void OnDisable()
        {
            BattleCharacter.OnWeapon += SetWeapon;
            base.OnDisable();
        }

        private void SetWeapon(WeaponItemDataSO data)
        {
            haveWeapon.SetActive(true);
            weaponRenderer.sprite = data.itemImage;
        }
    }
}