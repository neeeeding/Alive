using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.Inventory.Item;
using _02Script.Manager;
using UnityEngine;

namespace _02Script.Battle.Entity
{
    public class BattleCharacterManager : MonoBehaviour
    {
        [SerializeField] private WeaponItemDataSO[] allWeaponSO;
        
        [SerializeField] private List<BattleEntity> characters = new List<BattleEntity>();
        [SerializeField] private MonsterManager monsterManager;

        private void OnEnable()
        {
            monsterManager.SetTargetList(characters);
            GameManager.OnStart += SetStartCharacter;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= SetStartCharacter;
        }

        private void SetStartCharacter()
        {
            foreach (BattleEntity character in characters)
            {
                (character as BattleCharacter).SetCharacter(allWeaponSO[0]);
            }
        }
    }
}