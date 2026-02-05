using _02Script.GamePlayer.State;

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
    }
}