using UnityEngine;

namespace _02Script.Player.State
{
    public class PIdleState : PState
    {
        public PIdleState(string animation, PStateMachine machine, Player player) : base(animation, machine, player)
        {
        }
    }
}
