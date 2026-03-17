using System.Collections.Generic;
using _02Script.Battle.UI.Armor;
using _02Script.Battle.UI.Etc;
using _02Script.Battle.UI.Job;
using _02Script.Battle.UI.Weapon;
using UnityEngine;

namespace _02Script.Battle.Entity
{
    public class BattleCharacterManager : MonoBehaviour
    {
        public List<BattleEntity> characters = new List<BattleEntity>();
        [SerializeField] private List<WeaponInventory> weaponInventory = new List<WeaponInventory>();
        [SerializeField] private List<ArmorInventory> armorInventory = new List<ArmorInventory>();
        [SerializeField] private List<ForCharacterUI> forCharacterUI = new List<ForCharacterUI>();
        [SerializeField] private List<SkillBtn> skillBtn = new List<SkillBtn>();

        private void OnEnable()
        {
            SelectDistribution.OnStart += SetStartCharacter;
        }

        private void OnDisable()
        {
            SelectDistribution.OnStart -= SetStartCharacter;
        }

        private void SetStartCharacter()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                weaponInventory[i].SetInventoryCharacter(characters[i] as BattleCharacter);
                armorInventory[i].SetInventoryCharacter(characters[i] as BattleCharacter);
                (characters[i] as BattleCharacter).SetCharacter(forCharacterUI[i]);
                forCharacterUI[i].SetEntity(characters[i].ReturnName());
                skillBtn[i].SetEntity(characters[i]);
                characters[i].gameObject.SetActive(true);
            }
        }
    }
}