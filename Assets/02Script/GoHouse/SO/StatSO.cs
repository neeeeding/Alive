using System;
using _02Script.UI.Person;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/GoHouse/Block/StatSO")]
    public class StatSO : BlockActionSO
    {
        public static Action<StatsType,int> OnStat;
        
        [SerializeField] private StatsType stat;
        [SerializeField] private int addValue;
        public override void DoBlockAction()
        {
            OnStat?.Invoke(stat,addValue);
        }
    }
}