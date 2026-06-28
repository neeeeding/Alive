using _02Script.GamePlayer.GamePlayer;

namespace _02Script.GamePlayer.State
{
    public class PSkillState : PState
    {
        public PSkillState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }

        public override void Enter(int x, int y)
        {
            base.Enter(x, y);
            foreach (string b in AnimBool)
            {
                Player.Animator.SetBool(b, false);
            }
            Player.Animator.SetBool(AnimBool[3], true); // Skill = true

            if (Player.PlayerMovement != null)
            {
                Player.PlayerMovement.SetAttacking(true);
            }
        }

        public override void Exit()
        {
            base.Exit();
            foreach (string b in AnimBool)
            {
                Player.Animator.SetBool(b, false);
            }

            if (Player.PlayerMovement != null)
            {
                Player.PlayerMovement.SetAttacking(false);
            }
        }
    }
}
