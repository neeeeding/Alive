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
            _player.Animator.SetBool(MoveAnimBool,true);

            if (_player.transform.position.x == PlayerMovement.Instance.TargetPos.x && _player.transform.position.y == PlayerMovement.Instance.TargetPos.y)
            {
                StateMachine.ChangeState(PlayerState.Idle, x,y);
            }
        }

        public override void StateFixedUpdate() //움직임
        {
            base.StateFixedUpdate();

            if(Vector2.Distance(_player.transform.position, PlayerMovement.Instance.TargetPos) < 0.5f)
            {
                StateMachine.ChangeState(PlayerState.Idle,(int)_player.Animator.GetFloat(X),(int)_player.Animator.GetFloat(Y));
            }
        }
        public override void Exit()
        {
            base.Exit();
            _player.Animator.SetBool(MoveAnimBool,false);
        }
    }
}