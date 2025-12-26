using UnityEngine;

namespace _02Script.Player.State
{
    public class PState
    {
        protected Player _player; //플레이어
        protected PStateMachine StateMachine; //머신

        protected readonly string X = "X";
        protected readonly string Y = "Y";

        public PState(string animation,PStateMachine machine,Player player)
        {
            StateMachine = machine;
            _player = player;
        }

        public virtual void Enter(int x, int y)
        {
            _player.Animator.SetFloat(X, x);
            _player.Animator.SetFloat(Y, y);
        }

        public virtual void Exit()
        {
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
        Idle, Move, Chat, hold
    }
}