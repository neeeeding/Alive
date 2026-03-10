using System.Collections.Generic;
using _02Script.Battle.Monster;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Battle.Stage
{
    public class StageMonsterSet : SetMonsterSpawn
    {
        [SerializeField] private BattleStageSO stage;
        
        private List<MonsterSO> _monster = new List<MonsterSO>();
        private List<float> _mTime = new List<float>();
        private List<Vector3> _mPos = new List<Vector3>();

        protected override void ToOrganizeList()
        {
            foreach (KeyValuePair<Monster.Monster, float> spawn in setSpawn)
            {
                _monster.Add(spawn.Key.GetMonsterType());
                _mTime.Add(spawn.Value);
                _mPos.Add(spawn.Key.transform.position);
            }
        }

        protected override void GiveList()
        {
            stage.SetMonster(_monster,_mTime,_mPos);
            print("ok Monster");
            gameObject.SetActive(false);
        }
    }
}