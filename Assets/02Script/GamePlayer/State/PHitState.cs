using _02Script.GamePlayer.GamePlayer;

namespace _02Script.GamePlayer.State
{
    public class PHitState : PState
    {
        public PHitState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }

        public override void Enter(int x, int y)
        {
            base.Enter(x,y);
            foreach (string b in AnimBool)
            {
                Player.Animator.SetBool(b,false);
            }
            Player.Animator.SetBool(AnimBool[5],true);
        }

        public override void Exit()
        {
            base.Exit();
            foreach (string b in AnimBool)
            {
                Player.Animator.SetBool(b,false);
            }
        }
    }
}