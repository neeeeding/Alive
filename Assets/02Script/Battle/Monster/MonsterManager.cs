using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.Entity;
using _02Script.Battle.UI.Job;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class MonsterManager : MonoBehaviour
    {
        public static Action OnSuccess;
        
        [SerializeField] private Transform parent;
        [SerializeField] private Monster monsterPrefab;
        [SerializeField] private BossMonster bossMonsterPrefab;
        
        private List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)> _monsterSpawnList
            = new List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)>();
        private List<Monster> _monsters = new List<Monster>();
        private List<BossMonster> _bossMonsters = new List<BossMonster>();
        private List<BattleEntity> _canTargets = new List<BattleEntity>();

        private int _curAlive;
        private bool _isSpawn;
        private float _curTime;

        private void OnEnable()
        {
            _curAlive = 0;
            _isSpawn = false;
            _curTime = 0;
            Monster.OnDie += AddMonsterList;
            SelectDistribution.OnStart += SetStart;
        }

        private void OnDisable()
        {
            Monster.OnDie -= AddMonsterList;
            SelectDistribution.OnStart -= SetStart;
        }

        private void AddMonsterList(Monster monster)
        {
            if (monster is BossMonster boss)
            {
                _bossMonsters.Add(boss);
            }
            else
            {
                _monsters.Add(monster);
            }
            
            monster.gameObject.SetActive(false);
            _curAlive--;
            
            // [수정] 스폰 종료 여부 및 살아있는 몬스터 수 체크
            if (_curAlive <= 0 && (!_isSpawn || _monsterSpawnList.Count <= 0))
                Victory();
        }

        #region Spawn
        private void SpawnMonster()
        {
            if (!_isSpawn) return;

            // [수정] 모든 몬스터가 스폰되었을 때
            if (_monsterSpawnList.Count <= 0)
            {
                _isSpawn = false;
                if (_curAlive <= 0)
                {
                    Victory();
                }
                return;
            }
            
            List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)> spawnList = 
                new List<(MonsterSO monsterType, Vector3 spawnPos, float spawnDelay)>();
            for (int i = 0; i < _monsterSpawnList.Count; i++)
            {
                if (_monsterSpawnList[i].spawnDelay <= _curTime)
                {
                    spawnList.Add((_monsterSpawnList[i]));
                }
            }

            foreach ((MonsterSO monsterType, Vector3 spawnPos, float spawnDelay) spawn in spawnList)
            {
                _curAlive++;
                _monsterSpawnList.Remove(spawn);
                
                Monster monster = null;
            
                if (spawn.monsterType as BossMonsterSO)
                {
                    if (_bossMonsters.Count <= 0)
                        NewMonster(spawn.monsterType);
                    
                    monster = _bossMonsters[0];
                    _bossMonsters.RemoveAt(0);
                }
                else
                {
                    if (_monsters.Count <= 0)
                        NewMonster(spawn.monsterType);
                    
                    monster = _monsters[0];
                    _monsters.RemoveAt(0);
                }
                
                monster.transform.position = spawn.spawnPos;
                foreach (BattleEntity target in _canTargets)
                {
                    (target as BattleCharacter).GetCanTargets(monster);
                }
                
                monster.gameObject.SetActive(true);
            }
        }

        private void NewMonster(MonsterSO monsterType)
        {
            if (monsterType is BossMonsterSO bossSo)
            {
                BossMonster monster = Instantiate(bossMonsterPrefab, parent);
                monster.SetMonster(bossSo);
                _bossMonsters.Add(monster);
                monster.GetCanTargets(_canTargets.ToList());
                monster.gameObject.SetActive(false);
            }
            else
            {
                Monster monster = Instantiate(monsterPrefab, parent);
                monster.SetMonster(monsterType);
                monster.GetCanTargets(_canTargets.ToList());
                _monsters.Add(monster);
                monster.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!_isSpawn) return;
            _curTime += Time.deltaTime;
            SpawnMonster();
        }
        #endregion

        #region Set
        public void SetTargetList(List<BattleEntity> targetList)
        {
            _canTargets = targetList;
        }
        public void SetSpawnList(List<(MonsterSO, Vector3, float)> monsterSpawnList)
        {
            _monsterSpawnList = monsterSpawnList;
        }
        private void SetStart()
        {
            _isSpawn = true;
        }
        #endregion

        private void Victory()
        {
            OnSuccess?.Invoke();
        }
    }
}
