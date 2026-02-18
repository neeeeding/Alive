using _02Script.Battle;
using _02Script.GamePlayer.State;
using _02Script.UI.person;

namespace _02Script.GamePlayer.GamePlayer
{
    public class BattleMonster : Player
    {
        protected override void Awake()
        {
            base.Awake();
            stateMachine.AddState(PlayerStateType.Attack, new PAttackState("Attack", stateMachine, this));
            stateMachine.AddState(PlayerStateType.Skill, new PSkillState("Skill", stateMachine, this));
            stateMachine.AddState(PlayerStateType.Die, new PDieState("Die", stateMachine, this));
            stateMachine.AddState(PlayerStateType.Hit, new PHitState("Hit", stateMachine, this));
        }

        protected override  void AddStats(StatsType type, int add) //스탯
        {
            if(!isCurPlayer) return;
            
            BattleSaveManager.Instance.PlayerStat.characterStats[playerName][type] += add;
        }
    }
}