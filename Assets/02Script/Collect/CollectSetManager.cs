using System.Collections.Generic;
using _02Script.Battle;
using _02Script.Battle.UI.Job;
using _02Script.Collect.Arrow;
using _02Script.GamePlayer.GamePlayer;
using UnityEngine;

namespace _02Script.Collect
{
    public class CollectSetManager : MonoBehaviour
    {
        public List<CollectPlayer> characters = new List<CollectPlayer>();
        [SerializeField] private List<CollectInventory> collectInventory = new List<CollectInventory>();
        [SerializeField] private List<DeleteBtn> delete = new List<DeleteBtn>();
        [SerializeField] private List<ArrowMove> arrow = new List<ArrowMove>();
        [SerializeField] private ArrowManager arrowManager;

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
                collectInventory[i].SetInventoryCharacter(characters[i].playerName);
                arrow[i].SetCharacter(characters[i].gameObject, characters[i].MyColor,arrowManager);
                delete[i].SetCharacter(characters[i].playerName);
                characters[i].gameObject.SetActive(true);
            }
        }
    }
}