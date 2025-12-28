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

            if (_player.transform.position.x == _player.PlayerMovement.TargetPos.x && _player.transform.position.y == _player.PlayerMovement.TargetPos.y)
            {
                StateMachine.ChangeState(PlayerState.Idle, x,y);
            }
        }

        public override void StateFixedUpdate() //움직임
        {
            base.StateFixedUpdate();

            if(Vector2.Distance(_player.transform.position, _player.PlayerMovement.TargetPos) < 0.5f)
            {
                _player.Animator.SetBool(MoveAnimBool,false);
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