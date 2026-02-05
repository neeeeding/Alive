using _02Script.GamePlayer.State;

namespace _02Script.GamePlayer.GamePlayer
{
    public class CollectPlayer : Player
    {
        protected override void Awake()
        {
            base.Awake();
            stateMachine.AddState(PlayerStateType.Collect, new PCollectState("Collect", stateMachine, this));
        }
    }
}