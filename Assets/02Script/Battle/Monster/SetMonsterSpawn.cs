using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class SetMonsterSpawn : MonoBehaviour
    {
        protected List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)> _finishList 
            = new List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)>();
        
        [SerializeField] protected SerializedDictionary<Monster, float> setSpawn;
        [SerializeField] protected MonsterManager manager;

        private void Awake()
        {
            ToOrganizeList();
            GiveList();
        }

        protected virtual void ToOrganizeList()
        {
            foreach (KeyValuePair<Monster, float> spawn in setSpawn)
            {
                _finishList.Add((spawn.Key.GetMonsterType(), spawn.Key.transform.position, spawn.Value));
            }
        }

        protected virtual void GiveList()
        {
            manager.SetSpawnList(_finishList);
            gameObject.SetActive(false);
        }
    }
}