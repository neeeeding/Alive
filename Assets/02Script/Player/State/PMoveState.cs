using UnityEngine;

namespace _02Script.Player.State
{
    public class PMoveState : PState
    {
        public PMoveState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }

        public override void Enter(int x, int y)
        {
            base.Enter(x,y);
            Player.Animator.SetBool(MoveAnimBool,true);

            if (Player.transform.position.x == Player.PlayerMovement.TargetPos.x && Player.transform.position.y == Player.PlayerMovement.TargetPos.y)
            {
                StateMachine.ChangeState(PlayerState.Idle, x,y);
            }
        }

        public override void StateFixedUpdate() //움직임
        {
            base.StateFixedUpdate();

            if(Vector2.Distance(Player.transform.position, Player.PlayerMovement.TargetPos) < 0.5f)
            {
                Player.Animator.SetBool(MoveAnimBool,false);
                StateMachine.ChangeState(PlayerState.Idle,(int)Player.Animator.GetFloat(X),(int)Player.Animator.GetFloat(Y));
            }
        }
        public override void Exit()
        {
            base.Exit();
            Player.Animator.SetBool(MoveAnimBool,false);
        }
    }
}