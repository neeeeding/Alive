using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class SetMonsterSpawn : MonoBehaviour
    {
        private List<(Monster monsterType, Transform spawnPos, float spawnDelay)> _finishList 
            = new List<(Monster monsterType, Transform spawnPos, float spawnDelay)>();
        
        [SerializeField] private SerializedDictionary<Monster, float> setSpawn;
        [SerializeField] private MonsterManager manager;

        private void Awake()
        {
            ToOrganizeList();
            GiveList();
        }

        private void ToOrganizeList()
        {
            foreach (KeyValuePair<Monster, float> spawn in setSpawn)
            {
                _finishList.Add((spawn.Key, spawn.Key.transform, spawn.Value));
            }
        }

        private void GiveList()
        {
            manager.SetSpawnList(_finishList);
        }
    }
}