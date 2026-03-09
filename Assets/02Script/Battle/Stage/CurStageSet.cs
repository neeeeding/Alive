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
        
        //몬스터 스폰에 대해
        private List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)> _monsterSpawn = new List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)>();
        
        //아이템
        private List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)> _itemSpawn = new List<(ItemDataSO monsterType, Vector3 spawnPos, int spawnCount)>();

        private void Awake()
        {
            SetList();
        }

        private void SetList()
        {
            foreach (KeyValuePair<MonsterSO, List<float>> spawn in curStage.monsterSpawn.ToDictionary())
            {
                for (int i = 0; i < spawn.Value.Count; i++)
                {
                    _monsterSpawn.Add((spawn.Key, curStage.monsterPos[spawn.Key][i], spawn.Value[i]));
                }
            }
            foreach (KeyValuePair<ItemDataSO, List<int>> spawn in curStage.itemSpawn.ToDictionary())
            {
                for (int i = 0; i < spawn.Value.Count; i++)
                {
                    _itemSpawn.Add((spawn.Key, curStage.itemPos[spawn.Key][i], spawn.Value[i]));
                }
            }
            
            monsterManager.SetSpawnList(_monsterSpawn);
            itemManager.SetSpawnList(_itemSpawn);
        }
    }
}