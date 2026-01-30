using UnityEngine;

namespace _02Script.Player.State
{
    public class PIdleState : PState
    {
        public PIdleState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }

        public override void Enter(int x, int y)
        {
            PlayerInput.OnMousePos += Move;
            PlayerInput.OnMovePos += Move;
        }

        private void Move(Vector2 mousePos)
        {
            if (mousePos != Vector2.zero)
            {
                StateMachine.ChangeState(PlayerState.Move,(int)Player.Animator.GetFloat(X),(int)Player.Animator.GetFloat(Y));
            }
        }

        public override void Exit()
        {
            PlayerInput.OnMousePos -= Move;
            PlayerInput.OnMovePos -= Move;
        }
    }
}
