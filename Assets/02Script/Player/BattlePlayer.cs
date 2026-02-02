using System;
using System.Collections.Generic;
using _02Script.Etc;
using _02Script.Manager;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Player
{
    public class BattlePlayer : Player
    {
        [Header("BattlePlayer--")]
        [SerializeField] private Color outlineColor;
        [SerializeField] private SpriteRenderer outline;

        private readonly float _baseAttackCoolTime = 2f;
        private Dictionary<StatsType, float> _stats = new Dictionary<StatsType, float>();

        #region EnDi
        private void OnEnable()
        {
            GameManager.OnStart += SetStats;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.OnStart -= SetStats;
        }
        #endregion

        private void SetStats()
        {
            Dictionary<StatsType, int> stats = GameManager.Instance.PlayerStat.characterStats[playerName].ToDictionary();
            _stats.Clear();
            foreach (StatsType sta in Enum.GetValues(typeof(StatsType)))
            {
                _stats.Add(sta,StatCalculate.Calculate(playerName, sta));
            }
        }
    }
}