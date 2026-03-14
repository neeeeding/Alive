using _02Script.GoHouse.SO;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.GoHouse.Etc
{
    public class GoHouseStat : MonoBehaviour
    {
        private void OnEnable()
        {
            StatSO.OnStat += AddStats;
        }

        private void OnDisable()
        {
            StatSO.OnStat -= AddStats;   
        }

        private void AddStats(StatsType type, int add) //스탯
        {
            GoHouseSaveManager.Instance.PlayerStat.characterStats
                [(EntityName)(Random.Range(10,14) * 1000)][type] += add;
        }
    }
}