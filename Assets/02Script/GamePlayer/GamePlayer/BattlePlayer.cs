using _02Script.Battle;
using _02Script.Battle.Entity;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using UnityEngine;

namespace _02Script.GamePlayer.GamePlayer
{
    public class BattlePlayer : BattleMonster
    {
        [SerializeField] private SpriteRenderer weaponRenderer;
        [SerializeField] private GameObject haveWeapon;
        
        private void OnEnable()
        {
            BattleCharacter.OnChangeWeapon += SetWeapon;
        }

        protected override void OnDisable()
        {
            BattleCharacter.OnChangeWeapon -= SetWeapon;
            base.OnDisable();
        }

        private void SetWeapon(WeaponItemDataSO data)
        {
            haveWeapon.SetActive(true);
            weaponRenderer.sprite = data.itemImage;
        }

        protected override  void AddStats(EntityName name,StatsType type, int add) //스탯
        {
            if(name != playerName) return;
            
            BattleSaveManager.Instance.PlayerStat.characterStats[playerName][type] += add;
        }
    }
}