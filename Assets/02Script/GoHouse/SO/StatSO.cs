using System;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/GoHouse/StatSO")]
    public class StatSO : BlockActionSO
    {
        public static Action<StatsType,float, BlockActionSO> OnStat;
        
        [SerializeField] private StatsType stat;
        [SerializeField] private float addValue;
        public override void DoBlockAction()
        {
            OnStat?.Invoke(stat,addValue,this);
        }
    }
}