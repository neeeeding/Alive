using UnityEngine;

namespace _02Script.Player.State
{
    public class PIdleState : PState
    {
        public PIdleState(string animation, PStateMachin machin, Player player) : base(animation, machin, player)
        {
        }

        public override void Enter(PlayerRotate rotate)
        {
            base.Enter(rotate);
            PlayerMobileInput.mousePos += Move;
        }

        private void Move(Vector2 mousePos)
        {
            if (mousePos != Vector2.zero)
            {
                _stateMachin.ChangeState(PlayerState.Move,
                    Mathf.Abs(mousePos.x) <= Mathf.Abs(mousePos.y) ?
                        mousePos.y <= 0 ? PlayerRotate.Back : PlayerRotate.Front :
                        mousePos.x <= 0 ? PlayerRotate.Left : PlayerRotate.Right);
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerMobileInput.mousePos -= Move;
        }
    }
}
