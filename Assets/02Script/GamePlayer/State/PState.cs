using _02Script.GamePlayer.GamePlayer;

namespace _02Script.GamePlayer.State
{
    public class PState
    {
        protected Player Player; //플레이어
        protected PStateMachine StateMachine; //머신

        protected readonly string X = "X";
        protected readonly string Y = "Y";
        
        protected readonly string[] AnimBool =
        {
            "Move",
            "Collect",
            "Attack",
            "Skill",
            "Die",
            "Hit"
        };

        public PState(string animation,PStateMachine machine,Player player)
        {
            StateMachine = machine;
            Player = player;
        }

        public virtual void Enter(int x, int y)
        {
            foreach (string b in AnimBool)
            {
                Player.Animator.SetBool(b,false);
            }
            
            Player.Animator.SetFloat(X, x);
            Player.Animator.SetFloat(Y, y);
        }

        public virtual void Exit()
        {
            foreach (string b in AnimBool)
            {
                Player.Animator.SetBool(b,false);
            }
        }

        public virtual void StateUpdate()
        {

        }

        public virtual void StateFixedUpdate()
        {

        }
    }

    public enum PlayerStateType
    {
        Idle = 0,
        Move,
        Collect,
        Attack,
        Skill,
        Die,
        Hit,
    }
}