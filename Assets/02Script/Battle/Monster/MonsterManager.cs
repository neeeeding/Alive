using System.Collections.Generic;
using _02Script.Battle.Entity;
using UnityEngine;

namespace _02Script.Battle.Monster
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private BossMonsterHpUI bossHpUI;
        [SerializeField] private Transform parent;
        [SerializeField] private Monster monsterPrefab;
        [SerializeField] private BossMonster bossMonsterPrefab;
        
        private List<(MonsterSO monsterType, Transform spawnPos, float spawnDelay)> _monsterSpawnList; //종류, 위치, 스폰될 타이밍
        private List<Monster> _monsters = new List<Monster>();
        private List<BossMonster> _bossMonsters = new List<BossMonster>();
        private List<BattleEntity> canTargets = new List<BattleEntity>(); //타겟 후보

        private int _curAlive; //살아있는 수
        private bool _isSpawnStop; // 생성 종료인지
        private float _curTime;

        private void OnEnable()
        {
            _curAlive = 0;
            _isSpawnStop = true;
            _curTime = 0;
            Monster.OnDie += AddMonsterList;
        }

        private void OnDisable()
        {
            Monster.OnDie -= AddMonsterList;
        }

        private void AddMonsterList(Monster monster) //풀링
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
            
            List<(MonsterSO monsterType, Transform spawnPos, float spawnDelay)> spawnList = new List<(MonsterSO monsterType, Transform spawnPos, float spawnDelay)>();
            for (int i = 0; i < _monsterSpawnList.Count; i++)
            {
                if (_monsterSpawnList[i].spawnDelay <= _curTime)
                {
                    spawnList.Add((_monsterSpawnList[i]));
                }
            }

            foreach ((MonsterSO monsterType, Transform spawnPos, float spawnDelay) spawn in spawnList)
            {
                _curAlive++;
                _monsterSpawnList.Remove(spawn);
                
                Monster monster = null;
            
                //리스트 때문
                if (spawn.monsterType as BossMonsterSO)
                {
                    if (_bossMonsters.Count <= 0)
                        NewMonster(spawn.monsterType);
                    
                    monster = _bossMonsters[0];
                }
                else
                {
                    if (_monsters.Count <= 0)
                        NewMonster(spawn.monsterType);
                    
                    monster = _monsters[0];
                }
                
                monster.transform.position = spawn.spawnPos.position;
                monster.gameObject.SetActive(true);
            }
        }
        private void NewMonster(MonsterSO monsterType)
        {
            if (monsterType is BossMonsterSO bossSo)
            {
                BossMonster monster = Instantiate(bossMonsterPrefab, parent);
                
                monster.SetMonster(bossSo, bossHpUI);
                _bossMonsters.Add(monster);
                monster.GetCanTargets(canTargets);
                monster.gameObject.SetActive(false);
                foreach (BattleEntity target in canTargets)
                {
                    (target as BattleCharacter).GetCanTargets(monster);
                }
            }
            else
            {
                Monster monster = Instantiate(monsterPrefab, parent);
                monster.SetMonster(monsterType);
                monster.GetCanTargets(canTargets);
                _monsters.Add(monster);
                monster.gameObject.SetActive(false);
                foreach (BattleEntity target in canTargets)
                {
                    (target as BattleCharacter).GetCanTargets(monster);
                }
            }
        }
        private void Update()
        {
            _curTime += Time.deltaTime;
            SpawnMonster();
        }
        #endregion

        public void SetTargetList(List<BattleEntity> targetList)
        {
            canTargets = targetList;
        }
        public void SetSpawnList(List<(MonsterSO, Transform, float)> monsterSpawnList)
        {
            _monsterSpawnList = monsterSpawnList;
        }

        private void Victory()
        {
            // 성공
        }
    }
}