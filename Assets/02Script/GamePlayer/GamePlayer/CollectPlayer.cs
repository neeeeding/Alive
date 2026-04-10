using _02Script.Battle;
using _02Script.GamePlayer.State;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using UnityEngine;

namespace _02Script.GamePlayer.GamePlayer
{
    public class CollectPlayer : Player
    {
        [SerializeField] private Color myColor;
        
        public Color MyColor { get => myColor;}
        protected override void Awake()
        {
            base.Awake();
            stateMachine.AddState(PlayerStateType.Collect, new PCollectState("Collect", stateMachine, this));
        }

        protected override  void AddStats(EntityName name,StatsType type, int add) //스탯
        {
            if(name != playerName) return;
            
            BattleSaveManager.Instance.PlayerStat.characterStats[playerName][type] += add;
        }
    }
}