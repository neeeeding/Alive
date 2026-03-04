using System.Collections.Generic;
using _02Script.Battle;
using _02Script.Collect.Arrow;
using _02Script.GamePlayer.GamePlayer;
using UnityEngine;

namespace _02Script.Collect
{
    public class CollectManager : MonoBehaviour
    {
        [SerializeField] private List<CollectPlayer> characters = new List<CollectPlayer>();
        [SerializeField] private List<CollectInventory> collectInventory = new List<CollectInventory>();
        [SerializeField] private List<ArrowMove> arrow = new List<ArrowMove>();
        [SerializeField] private ArrowManager arrowManager;

        private void OnEnable()
        {
            BattleSaveManager.OnStart += SetStartCharacter;
        }

        private void OnDisable()
        {
            BattleSaveManager.OnStart -= SetStartCharacter;
        }

        private void SetStartCharacter()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                collectInventory[i].SetInventoryCharacter(characters[i].playerName);
                arrow[i].SetCharacter(characters[i].gameObject, characters[i].MyColor,arrowManager);
            }
        }
    }
}