using System;
using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.Battle.UI.Etc;
using _02Script.Battle.UI.Job;
using _02Script.Collect.Item;
using _02Script.InGameDebug;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    public class CurStageSet : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField] private BattleStageSO[] stage;
        [SerializeField] private BattleStageSO curStage;
        
        [Header("Need(script setting)")]
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private CollectItemManager itemManager;
        [SerializeField] private GameMiddleUI middleUI;
        
        [Header("PlayerPos")]
        [SerializeField] private Transform cPlayerOnePos;
        [SerializeField] private Transform cPlayerTwoPos;
        [SerializeField] private Transform bPlayerOnePos;
        [SerializeField] private Transform bPlayerTwoPos;
        
        [Header("CamPos")]
        [SerializeField] private Transform cCamPos;
        [SerializeField] private Transform bCamPos;
        [SerializeField] private Transform miniCamPos;
        
        [SerializeField] private BoxCollider2D cCamLimit;
        [SerializeField] private BoxCollider2D bCamLimit;

        private readonly string _goHouseSoSave = "battle_GoHouseStageSoSave";
        private readonly string _battleSOSave = "battle_BattleStageSoSave";
        
        //몬스터 스폰에 대해
        private List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)> _monsterSpawn = new List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)>();
        
        //아이템
        private List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)> _itemSpawn = new List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)>();

        private void OnEnable()
        {
            LoadStage();
            SelectDistribution.OnStart += StartSet; 
            SetList();
            SetEtcPos();
        }

        private void OnDisable()
        {
            SelectDistribution.OnStart -= StartSet;
        }

        private void StartSet() //게임이 시작되고 세팅
        {
            SetTime();
            SaveGoHouseScene();
            
            gameObject.SetActive(false);
        }

        public void SetPlayer(Dictionary<SelectCharacterType,List<Transform>> allPlayer) // '플레이어'를 받아오기
        {
            cPlayerOnePos = allPlayer[SelectCharacterType.Collect][0];
            cPlayerTwoPos = allPlayer[SelectCharacterType.Collect][1];
            bPlayerOnePos = allPlayer[SelectCharacterType.Battle][0];
            bPlayerTwoPos = allPlayer[SelectCharacterType.Battle][1];

            SetPlayerPos();
        }
        
        private void SetList() //몬스터 & 아이템
        {
            for (int i = 0; i < curStage.monster.Count; i++)
            {
                _monsterSpawn.Add((curStage.monster[i], curStage.mPos[i], curStage.mTime[i]));
            }
            for (int i = 0; i < curStage.itme.Count; i++)
            {
                _itemSpawn.Add((curStage.itme[i], curStage.iPos[i], curStage.iCount[i]));
            }
            
            monsterManager.SetSpawnList(_monsterSpawn);
            itemManager.SetSpawnList(_itemSpawn);
        }

        private void SetEtcPos() //플레이어 외 위치들
        {
            cCamPos.position = curStage.cCamPos;
            bCamPos.position = curStage.bCamPos;
            miniCamPos.position = curStage.miniCamPos;
            
            cCamLimit.offset = curStage.cCamLimitOffset;
            cCamLimit.size = curStage.cCamLimitSize;
            bCamLimit.offset = curStage.bCamLimitOffset;
            bCamLimit.size = curStage.bCamLimitSize;
        }

        private void SetPlayerPos() //플레이어 위치
        {
            cPlayerOnePos.position = curStage.cPlayerOnePos;
            cPlayerTwoPos.position = curStage.cPlayerTwoPos;
            bPlayerOnePos.position = curStage.bPlayerOnePos;
            bPlayerTwoPos.position = curStage.bPlayerTwoPos;
        }

        private void SetTime() //채집 시간
        {
            middleUI.SetTime(curStage.canCollectTime.x, curStage.canCollectTime.y);
        }
        private void LoadStage()
        {
            int index = PlayerPrefs.GetInt(_battleSOSave);
            //print(json);

            // curStage = ScriptableObject.CreateInstance<BattleStageSO>();
            // JsonUtility.FromJsonOverwrite(json, curStage);
            curStage = stage[index];
        }

        private void SaveGoHouseScene()
        {
            string json = JsonUtility.ToJson(curStage.goHouse);
            PlayerPrefs.SetString(_goHouseSoSave, json);
            PlayerPrefs.Save();
        }
    }
}