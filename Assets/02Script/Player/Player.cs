using _02Script.Player.State;
using UnityEngine;

namespace _02Script.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private string currentState;

        private Animator animator;
        public Animator Animator => animator;

        private PStateMachin stateMachin;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

            stateMachin = new PStateMachin();
            stateMachin.AddState(PlayerState.Move, new PMoveState("Move", stateMachin, this));
            stateMachin.AddState(PlayerState.Idle, new PIdleState("Idle", stateMachin, this));
            stateMachin.AddState(PlayerState.hold, new PHoldState("Hold", stateMachin, this));

            transform.position += Vector3.zero;
            stateMachin.ChangeState(PlayerState.Idle, PlayerRotate.Front);
        }

        private void OnDisable()
        {
            stateMachin.currentState.Exit();
        }

        public void ChangeState(PlayerState state)
        {
            stateMachin.ChangeState(state, PlayerRotate.Front);
        }

        private void Update()
        {
            stateMachin.currentState.StateUpdate();
            currentState = stateMachin.currentState.ToString();
        }

        private void FixedUpdate()
        {
            stateMachin.currentState.StateFixedUpdate();
        }
    }
}
