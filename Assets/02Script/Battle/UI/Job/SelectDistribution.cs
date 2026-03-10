using System.Collections.Generic;
using _02Script.Battle.Entity;
using _02Script.Battle.Monster;
using _02Script.Collect;
using _02Script.GamePlayer.GamePlayer;
using _02Script.Obj.Entity;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Battle.UI.Job
{
    public class SelectDistribution : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<EntityName,SelectCharacterCard> selectCard = new SerializedDictionary<EntityName,SelectCharacterCard>();
        [SerializeField] private SerializedDictionary<EntityName,BattleEntity> battleCharacter = new SerializedDictionary<EntityName,BattleEntity>();
        [SerializeField] private SerializedDictionary<EntityName,CollectPlayer> collectCharacters = new SerializedDictionary<EntityName,CollectPlayer>();
        
        [SerializeField] private BattleCharacterManager battleManager;
        [SerializeField] private CollectSetManager collectManager;
        [SerializeField] private MonsterManager monsterManager;

        private void OnEnable()
        {
            SelectCharacterCard.OnMouseUp += SelectComplete;
        }
        private void OnDisable()
        {
            SelectCharacterCard.OnMouseUp -= SelectComplete;
        }
        private void Awake()
        {
            SelectTime();
        }

        private void SelectComplete()
        {
            bool isComplete = true;
            foreach (KeyValuePair<EntityName, SelectCharacterCard> card in selectCard)
            {
                if (card.Value.Select == SelectCharacterType.None)
                {
                    isComplete = false;
                    break;
                }
            }
            
            if(!isComplete) return;
            
            Setting();
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        private void Setting()
        {
            List<BattleEntity> bc = new List<BattleEntity>();
            List<CollectPlayer> cc = new List<CollectPlayer>();
            
            battleManager.characters.Clear();
            collectManager.characters.Clear();
            
            foreach (KeyValuePair<EntityName, SelectCharacterCard> card in selectCard)
            {
                if (card.Value.Select == SelectCharacterType.Battle)
                {
                    battleManager.characters.Add(battleCharacter[card.Key]);
                }
                else if (card.Value.Select == SelectCharacterType.Collect)
                {
                    battleManager.characters.Add(battleCharacter[card.Key]);
                }
            }
            monsterManager.SetTargetList(battleManager.characters);
        }

        private void SelectTime()
        {
            Time.timeScale = 0;
        }
    }
}