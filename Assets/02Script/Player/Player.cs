using System;
using _02Script.Player.State;
using UnityEngine;

namespace _02Script.Player
{
    public class Player : MonoBehaviour
    {
        public static Action<Player> OnSelectPlayer;
        
        [SerializeField] private string currentState;

        private Animator animator;
        public Animator Animator => animator;

        private PStateMachine stateMachine;

        public bool isCurPlayer;

        public void Select()
        {
            OnSelectPlayer?.Invoke(this);
        }
        
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

            stateMachine = new PStateMachine();
            stateMachine.AddState(PlayerState.Move, new PMoveState("Move", stateMachine, this));
            stateMachine.AddState(PlayerState.Idle, new PIdleState("Idle", stateMachine, this));
            stateMachine.AddState(PlayerState.hold, new PHoldState("Hold", stateMachine, this));

            transform.position += Vector3.zero;
            stateMachine.ChangeState(PlayerState.Idle, PlayerRotate.Front);
        }

        private void OnDisable()
        {
            stateMachine.currentState.Exit();
        }

        public void ChangeState(PlayerState state)
        {
            stateMachine.ChangeState(state, PlayerRotate.Front);
        }

        private void Update()
        {
            stateMachine.currentState.StateUpdate();
            currentState = stateMachine.currentState.ToString();
        }

        private void FixedUpdate()
        {
            stateMachine.currentState.StateFixedUpdate();
        }
    }
}
