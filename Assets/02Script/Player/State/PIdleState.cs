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
            base.Enter(x,y);
            PlayerInput.OnMousePos += Move;
        }

        private void Move(Vector2 mousePos)
        {
            if (mousePos != Vector2.zero)
            {
                StateMachine.ChangeState(PlayerState.Move,(int)_player.Animator.GetFloat(X),(int)_player.Animator.GetFloat(Y));
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerInput.OnMousePos -= Move;
        }
    }
}
