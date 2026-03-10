using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.Collect.Item;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    public class CurStageSet : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField] private BattleStageSO curStage;
        
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private CollectItemManager itemManager;
        
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
        
        //몬스터 스폰에 대해
        private List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)> _monsterSpawn = new List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)>();
        
        //아이템
        private List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)> _itemSpawn = new List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)>();

        private void Awake()
        {
            SetList();
            SetPos();
        }

        private void SetList()
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

        private void SetPos()
        {
            cCamPos.position = curStage.cCamPos;
            bCamPos.position = curStage.bCamPos;
            miniCamPos.position = curStage.miniCamPos;
            
            cCamLimit.offset = curStage.cCamLimitOffset;
            cCamLimit.size = curStage.cCamLimitSize;
            bCamLimit.offset = curStage.bCamLimitOffset;
            bCamLimit.size = curStage.bCamLimitSize;
        }
    }
}