namespace _02Script.Player.State
{
    public class PState
    {
        protected Player Player; //플레이어
        protected PStateMachine StateMachine; //머신

        protected readonly string X = "X";
        protected readonly string Y = "Y";
        
        protected readonly string MoveAnimBool = "Move";

        public PState(string animation,PStateMachine machine,Player player)
        {
            StateMachine = machine;
            Player = player;
        }

        public virtual void Enter(int x, int y)
        {
            Player.Animator.SetBool(MoveAnimBool,false);
            
            Player.Animator.SetFloat(X, x);
            Player.Animator.SetFloat(Y, y);
        }

        public virtual void Exit()
        {
            Player.Animator.SetBool(MoveAnimBool,false);
        }

        public virtual void StateUpdate()
        {

        }

        public virtual void StateFixedUpdate()
        {

        }
    }

    public enum PlayerState
    {
        Idle, Move, Attack, Collect
    }
}