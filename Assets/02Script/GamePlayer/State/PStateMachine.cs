using System.Collections.Generic;

namespace _02Script.GamePlayer.State
{
    public class PStateMachine
    {
        public Dictionary<PlayerStateType, PState> PStateD = new Dictionary<PlayerStateType, PState>();

        public PState currentState;

        public void ChangeState(PlayerStateType stateType, int x, int y)
        {
            if(currentState != null)
            {
                currentState.Exit();
            }
            currentState = PStateD[stateType];
            currentState.Enter(x,y);
        }

        public void AddState(PlayerStateType stateType, PState sc)
        {
            PStateD.Add(stateType, sc);
        }
    }
}
