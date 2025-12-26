using System.Collections.Generic;

namespace _02Script.Player.State
{
    public class PStateMachine
    {
        public Dictionary<PlayerState, PState> PStateD = new Dictionary<PlayerState, PState>();

        public PState currentState;

        public void ChangeState(PlayerState state, int x, int y)
        {
            if(currentState != null)
            {
                currentState.Exit();
            }
            currentState = PStateD[state];
            currentState.Enter(x,y);
        }

        public void AddState(PlayerState state, PState sc)
        {
            PStateD.Add(state, sc);
        }
    }
}
