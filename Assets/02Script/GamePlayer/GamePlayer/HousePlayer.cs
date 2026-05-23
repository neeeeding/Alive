using System;
using System.Collections.Generic;
using _02Script.Etc;
using _02Script.Inventory.Inventory.Use;
using _02Script.Manager;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using UnityEngine;

namespace _02Script.GamePlayer.GamePlayer
{
    public class HousePlayer : Player
    {
        private Dictionary<StatsType, float> _stats = new Dictionary<StatsType, float>(); //스탯들
        private bool _isSetStat;
        
        protected void OnEnable()
        {
            HouseManager.OnStart += SetStats;
            UseWindow.OnGetStat += GetStat;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            HouseManager.OnStart -= SetStats;
            UseWindow.OnGetStat -= GetStat;
        }

        private void GetStat(EntityName name, StatsType stat, int value)
        {
            if(name != playerName) return;
            
            HouseManager.Instance.PlayerStat.characterStats[name][stat] += value;
            _stats[stat] = StatCalculate.Calculate(playerName, stat);
            
            if (stat == StatsType.curHp)
            {
                _stats[StatsType.curHp] = Mathf.Min(_stats[StatsType.curHp], _stats[StatsType.HpStat]);
                HouseManager.Instance.PlayerStat.characterStats[name][stat] = (int)_stats[StatsType.curHp];
            }
        }
        
        private void SetStats()
        {
            if(_isSetStat) return;
            
            _stats.Clear();
            
            foreach (StatsType sta in Enum.GetValues(typeof(StatsType)))
            {
                _stats.Add(sta,StatCalculate.Calculate(playerName, sta));
            }
            _isSetStat = true;
        }
    }
}
