using _02Script.GamePlayer.GamePlayer;

namespace _02Script.GamePlayer.State
{
    public class PIdleState : PState
    {
        public PIdleState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }
    }
}
