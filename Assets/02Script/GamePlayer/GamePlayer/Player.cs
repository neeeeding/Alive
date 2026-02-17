using System;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Obj.Entity;
using _02Script.GamePlayer.Movement;
using _02Script.GamePlayer.State;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.GamePlayer.GamePlayer
{
    public class Player: MonoBehaviour
    {
        public static Action<Player> OnSelectPlayer;
        
        [SerializeField] public EntityName playerName;
        [SerializeField] protected string currentState;
        
        public Animator Animator;
        
        public PlayerMovement PlayerMovement;

        protected PStateMachine stateMachine;

        public bool isCurPlayer;

        public void Select()
        {
            OnSelectPlayer?.Invoke(this);
        }
        
        protected virtual void Awake()
        {
            PlayerMovement =  GetComponent<PlayerMovement>();
            Animator = GetComponentInChildren<Animator>();
            
            ItemDataSO.OnStats += AddStats;

            transform.position += Vector3.zero;
            
            stateMachine = new PStateMachine();
            stateMachine.AddState(PlayerStateType.Move, new PMoveState("Move", stateMachine, this));
            stateMachine.AddState(PlayerStateType.Idle, new PIdleState("Idle", stateMachine, this));
            stateMachine.ChangeState(PlayerStateType.Idle, 0,0);
        }

        protected virtual  void AddStats(StatsType type, int add) //스탯
        {
            if(!isCurPlayer) return;
            
            HouseManager.Instance.PlayerStat.characterStats[playerName][type] += add;
        }

        protected virtual  void OnDisable()
        {
            ItemDataSO.OnStats -= AddStats;
            stateMachine.currentState.Exit();
        }

        public virtual  void ChangeState(PlayerStateType stateType, int x, int y)
        {
            stateMachine.ChangeState(stateType, x,y);
        }

        protected virtual  void Update()
        {
            stateMachine.currentState.StateUpdate();
            currentState = stateMachine.currentState.ToString();
        }

        protected virtual  void FixedUpdate()
        {
            stateMachine.currentState.StateFixedUpdate();
        }
    }
}