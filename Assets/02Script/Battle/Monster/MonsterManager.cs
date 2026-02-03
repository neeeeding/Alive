using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Monster monsterPrefab;
        [SerializeField] private BossMonster bossMonsterPrefab;
        
        private List<(Monster monsterType, Transform spawnPos, float spawnDelay)> _monsterSpawnList; //종류, 위치, 스폰될 타이밍
        private List<Monster> _monsters;
        private List<BossMonster> _bossMonsters;

        private int _curAlive; //살아있는 수
        private bool _isSpawnStop; // 생성 종료인지
        private float _curTime;

        private void OnEnable()
        {
            _curAlive = 0;
            _isSpawnStop = false;
            _curTime = 0;
            Monster.OnDie += AddMonsterList;
        }

        private void OnDisable()
        {
            Monster.OnDie -= AddMonsterList;
        }

        private void AddMonsterList(Monster monster) //풀링
        {
            if (monster as BossMonster)
            {
                _bossMonsters.Add(monster as BossMonster);
            }
            else
            {
                _monsters.Add(monster);
            }
            
            monster.gameObject.SetActive(false);
            _curAlive--;
            
            if(_curAlive <= 0 && _isSpawnStop)
                Victory();
        }

        #region Spawn
        private void SpawnMonster()
        {
            if (!_isSpawnStop || _monsterSpawnList.Count <= 0)
            {
                _isSpawnStop = false;
                return;
            }
            
            List<(Monster monsterType, Transform spawnPos, float spawnDelay)> spawnList = new List<(Monster monsterType, Transform spawnPos, float spawnDelay)>();
            for (int i = 0; i < _monsterSpawnList.Count; i++)
            {
                if (_monsterSpawnList[i].spawnDelay <= _curTime)
                {
                    spawnList.Add((_monsterSpawnList[i]));
                }
            }

            foreach ((Monster monsterType, Transform spawnPos, float spawnDelay) spawn in spawnList)
            {
                _curAlive++;
                _monsterSpawnList.Remove(spawn);
                
                Monster monster = new Monster();
            
                //리스트 때문
                if (spawn.monsterType as BossMonster)
                {
                    if (_bossMonsters.Count <= 0)
                        NewMonster(spawn.monsterType);
                    
                    monster = _bossMonsters[0];
                }
                else
                {
                    if (_monsters.Count <= 0)
                        NewMonster(spawn.monsterType);
                    
                    monster = _bossMonsters[0];
                }
                
                monster.transform.position = spawn.spawnPos.position;
                monster.gameObject.SetActive(true);
            }
        }
        private void NewMonster(Monster monsterType)
        {
            Monster monster = Instantiate(monsterType, parent);
            monster.gameObject.SetActive(false);
            
            if (monster as BossMonster)
            {
                _bossMonsters.Add(monster as BossMonster);
            }
            else
            {
                _monsters.Add(monster);
            }
        }
        private void Update()
        {
            _curTime += Time.deltaTime;
            SpawnMonster();
        }
        #endregion

        public void SetSpawnList(List<(Monster, Transform, float)> monsterSpawnList)
        {
            _monsterSpawnList = monsterSpawnList;
        }

        private void Victory()
        {
            // 성공
        }
    }
}