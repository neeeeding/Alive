using UnityEngine;

namespace _02Script.Player.State
{
    public class PMoveState : PState
    {
        public PMoveState(string animation, PStateMachin machin, Player player) : base(animation, machin, player)
        {
        }

        public override void Enter(PlayerRotate rotate)
        {
            base.Enter(rotate);

            if (_player.transform.position.x == PlayerMovement.Instance.TargetPos.x && _player.transform.position.y == PlayerMovement.Instance.TargetPos.y)
            {
                _stateMachin.ChangeState(PlayerState.Idle, _rotate);
            }
            ChangeAnimation(PlayerMovement.Instance.TargetPos);
        }

        private void ChangeAnimation(Vector2 mousePos) //애니 바꾸기
        {
            _player.Animator.SetBool(Animator.StringToHash(_rotate.ToString()), false); //일단 끄고

            Vector2 minusPos = new Vector2(mousePos.x - _player.transform.position.x, mousePos.y - _player.transform.position.y);


            _rotate = Mathf.Abs(minusPos.x) > Mathf.Abs(minusPos.y) ? //켜기
                minusPos.x <= 0 ? PlayerRotate.Left : PlayerRotate.Right :
                minusPos.y <= 0 ? PlayerRotate.Back : PlayerRotate.Front;

            _player.Animator.SetBool(Animator.StringToHash(_rotate.ToString()), true); //켜기
        }

        public override void StateFixedUpdate() //움직임
        {
            base.StateFixedUpdate();

            if(Vector2.Distance(_player.transform.position, PlayerMovement.Instance.TargetPos) < 0.5f)
            {
                _stateMachin.ChangeState(PlayerState.Idle,_rotate);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}