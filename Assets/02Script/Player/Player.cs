using System;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Player.State;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Player
{
    public class Player : MonoBehaviour
    {
        public static Action<Player> OnSelectPlayer;
        
        [SerializeField] private EntityName playerName;
        [SerializeField] private string currentState;

        private Animator animator;
        public Animator Animator => animator;
        
        private PlayerMovement playerMovement;
        public PlayerMovement PlayerMovement => playerMovement;

        private PStateMachine stateMachine;

        public bool isCurPlayer;

        public void Select()
        {
            OnSelectPlayer?.Invoke(this);
        }
        
        private void Awake()
        {
            ItemDataSO.OnStats += AddStats;
            
            playerMovement =  GetComponent<PlayerMovement>();
            animator = GetComponentInChildren<Animator>();

            stateMachine = new PStateMachine();
            stateMachine.AddState(PlayerState.Move, new PMoveState("Move", stateMachine, this));
            stateMachine.AddState(PlayerState.Idle, new PIdleState("Idle", stateMachine, this));
            stateMachine.AddState(PlayerState.hold, new PHoldState("Hold", stateMachine, this));

            transform.position += Vector3.zero;
            stateMachine.ChangeState(PlayerState.Idle, 0,0);
        }

        private void AddStats(StatsType type, int add) //스탯
        {
            if(!isCurPlayer) return;
            
            GameManager.Instance.PlayerStat.characterStats[playerName][type] += add;
        }

        private void OnDisable()
        {
            ItemDataSO.OnStats -= AddStats;
            stateMachine.currentState.Exit();
        }

        public void ChangeState(PlayerState state)
        {
            stateMachine.ChangeState(state, 0,0);
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
