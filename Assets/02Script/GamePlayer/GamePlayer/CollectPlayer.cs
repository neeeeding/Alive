using _02Script.Battle;
using _02Script.GamePlayer.State;
using _02Script.UI.person;

namespace _02Script.GamePlayer.GamePlayer
{
    public class CollectPlayer : Player
    {
        protected override void Awake()
        {
            base.Awake();
            stateMachine.AddState(PlayerStateType.Collect, new PCollectState("Collect", stateMachine, this));
        }

        protected override  void AddStats(StatsType type, int add) //스탯
        {
            if(!isCurPlayer) return;
            
            BattleSaveManager.Instance.PlayerStat.characterStats[playerName][type] += add;
        }
    }
}