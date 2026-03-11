using System;
using System.Collections.Generic;
using _02Script.Battle.Entity;
using _02Script.Battle.Monster;
using _02Script.Battle.Stage;
using _02Script.Collect;
using _02Script.GamePlayer.GamePlayer;
using _02Script.Obj.Entity;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Battle.UI.Job
{
    public class SelectDistribution : MonoBehaviour
    {
        public static Action OnStart;
        
        [SerializeField] private SerializedDictionary<EntityName,SelectCharacterCard> selectCard = new SerializedDictionary<EntityName,SelectCharacterCard>();
        [SerializeField] private SerializedDictionary<EntityName,BattleEntity> battleCharacter = new SerializedDictionary<EntityName,BattleEntity>();
        [SerializeField] private SerializedDictionary<EntityName,CollectPlayer> collectCharacters = new SerializedDictionary<EntityName,CollectPlayer>();
        
        [SerializeField] private BattleCharacterManager battleManager;
        [SerializeField] private CollectSetManager collectManager;
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private CurStageSet stageSet;

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
            OnStart?.Invoke();
            gameObject.SetActive(false);
        }

        private void Setting()
        {
            List<BattleEntity> bc = new List<BattleEntity>();
            List<CollectPlayer> cc = new List<CollectPlayer>();
            Dictionary<SelectCharacterType,List<Transform>> allPlayer = new Dictionary<SelectCharacterType,List<Transform>>();
            
            battleManager.characters.Clear();
            collectManager.characters.Clear();
            
            foreach (KeyValuePair<EntityName, SelectCharacterCard> card in selectCard)
            {
                if (card.Value.Select == SelectCharacterType.Battle)
                {
                    battleManager.characters.Add(battleCharacter[card.Key]);
                    if (!allPlayer.ContainsKey(SelectCharacterType.Battle))
                    {
                        allPlayer.Add(SelectCharacterType.Battle, new List<Transform>());
                    }
                    allPlayer[SelectCharacterType.Battle].Add(card.Value.transform);
                }
                else if (card.Value.Select == SelectCharacterType.Collect)
                {
                    collectManager.characters.Add(collectCharacters[card.Key]);
                    if (!allPlayer.ContainsKey(SelectCharacterType.Collect))
                    {
                        allPlayer.Add(SelectCharacterType.Collect, new List<Transform>());
                    }
                    allPlayer[SelectCharacterType.Collect].Add(card.Value.transform);
                }
            }
            stageSet.SetPlayer(allPlayer);
            monsterManager.SetTargetList(battleManager.characters);
        }

        private void SelectTime()
        {
            Time.timeScale = 0;
        }
    }
}