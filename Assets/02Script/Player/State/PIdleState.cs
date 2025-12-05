using UnityEngine;

namespace _02Script.Player.State
{
    public class PIdleState : PState
    {
        public PIdleState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }

        public override void Enter(PlayerRotate rotate)
        {
            base.Enter(rotate);
            PlayerInput.OnMousePos += Move;
        }

        private void Move(Vector2 mousePos)
        {
            if (mousePos != Vector2.zero)
            {
                StateMachine.ChangeState(PlayerState.Move,
                    Mathf.Abs(mousePos.x) <= Mathf.Abs(mousePos.y) ?
                        mousePos.y <= 0 ? PlayerRotate.Back : PlayerRotate.Front :
                        mousePos.x <= 0 ? PlayerRotate.Left : PlayerRotate.Right);
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerInput.OnMousePos -= Move;
        }
    }
}
