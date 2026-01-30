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
        }

        public override void Exit()
        {
            base.Exit();
            Player.Animator.SetBool(MoveAnimBool,false);
        }
    }
}