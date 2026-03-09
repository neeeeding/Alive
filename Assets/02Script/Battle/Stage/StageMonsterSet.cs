using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    public class StageMonsterSet : SetMonsterSpawn
    {
        [SerializeField] private BattleStageSO stage;
        
        public Dictionary<MonsterSO, List<float>> _monsterSpawn = new Dictionary<MonsterSO, List<float>>();
        public Dictionary<MonsterSO, List<Vector3>> _monsterPos = new Dictionary<MonsterSO, List<Vector3>>();

        protected override void ToOrganizeList()
        {
            foreach (KeyValuePair<Monster.Monster, float> spawn in setSpawn)
            {
                MonsterSO monster = spawn.Key.GetMonsterType();
                if (!_monsterPos.ContainsKey(monster))
                {
                    _monsterSpawn.Add(monster, new List<float>());
                    _monsterPos.Add(monster, new List<Vector3>());
                }
                
                _monsterSpawn[monster].Add(spawn.Value);
                _monsterPos[monster].Add(spawn.Key.transform.position);
            }
        }

        protected override void GiveList()
        {
            stage.SetMonster(_monsterSpawn,_monsterPos);
            print("ok Monster");
            gameObject.SetActive(false);
        }
    }
}