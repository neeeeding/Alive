using System.Collections.Generic;
using _02Script.Manager;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Etc
{
    public class StatCalculate : MonoBehaviour
    {
        private static Dictionary<StatsType,(float baseValue,float[] addValue,float lastAdd)> _allValues = new Dictionary<StatsType, (float, float[],float)>();

        private void Awake()
        {
            SetValue();
        }

        public static float Calculate(EntityName character,StatsType statType)
        {
            if (statType == StatsType.curHp) //체력은 계산할 필요가 없음.
            {
                return GameManager.Instance.PlayerStat.characterStats[character][StatsType.curHp];
            }
            
            int stat = GameManager.Instance.PlayerStat.characterStats[character][statType];
            
            float statValue = _allValues[statType].baseValue;

            if (stat >= 45 && _allValues[statType].lastAdd > 0)
            {
                //마지막 값 미리 더하기
                statValue += _allValues[statType].lastAdd
                             - _allValues[statType].addValue[_allValues[statType].addValue.Length - 1];
            }

            int addValue = (45/_allValues[statType].addValue.Length) +1;
            
            foreach (float value in _allValues[statType].addValue)
            {
                if(stat <= 1) break;
                statValue += (Mathf.Min(stat, addValue)-1) * value;
                stat -= (Mathf.Min(stat, addValue)-1);
            }
            
            return statValue;
        }

        private void SetValue()
        {
            _allValues.Clear();
            
            _allValues.Add(StatsType.HpStat,(50,new float[] {3,4,5,6,7,8,9,9,9},18));
            _allValues.Add(StatsType.attack,(3,new float[] {1,2,3,4,5},-1));
            _allValues.Add(StatsType.agility,(1,new float[] {1},6));
            _allValues.Add(StatsType.defense,(0,new float[] {1,2,2},4));
            _allValues.Add(StatsType.skill,(0,new float[] {1,2,3},6));
            _allValues.Add(StatsType.recovery,(0,new float[] {0.1f,0.5f,1},-1));
            _allValues.Add(StatsType.tolerance,(0,new float[] {1,2,3},6));
            _allValues.Add(StatsType.duration,(0,new float[] {1,2,3},6));
            _allValues.Add(StatsType.acceptance,(6,new float[] {1},-1));
            _allValues.Add(StatsType.mining,(0,new float[] {0.01f,0.05f,0.1f,0.25f,0.5f},-1));
        }
    }
}