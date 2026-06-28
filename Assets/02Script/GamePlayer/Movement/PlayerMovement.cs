using System;
using _02Script.GamePlayer.GamePlayer;
using _02Script.GamePlayer.State;
using _02Script.UI.InGame;
using UnityEngine;

namespace _02Script.GamePlayer.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed; // 속도
        [HideInInspector] public Vector2 TargetPos; // 갈 위치
        protected Rigidbody2D Rd;
        protected bool IsMoving;
        protected bool isAttacking; // 공격/스킬 동작 중인지
        
        protected Player player;

        protected readonly string X = "X";
        protected readonly string Y = "Y";
        protected Vector3 beforePos; // 위치 확인용

        public bool IsAttacking => isAttacking;

        #region endiaw

        protected virtual void Awake()
        {
            Rd = GetComponent<Rigidbody2D>();
            player = GetComponent<Player>();
        }

        protected virtual void OnEnable()
        {
            IsMoving = false;
            isAttacking = false;
            RunBtn.OnMoveSpeed += SetSpeed;
        }

        protected virtual void OnDisable()
        {
            RunBtn.OnMoveSpeed -= SetSpeed;
        }
        #endregion

        public void SetAttacking(bool attacking)
        {
            isAttacking = attacking;
            if (attacking)
            {
                StopMove();
            }
        }

        public virtual void StopMove()
        {
            IsMoving = false;
            if (Rd != null)
            {
                Rd.linearVelocity = Vector2.zero;
            }
        }

        protected virtual void FixedUpdate()
        {
            // 공격 중이면 회전 및 이동 전면 중단
            if (isAttacking)
            {
                if (Rd != null)
                {
                    Rd.linearVelocity = Vector2.zero;
                }
                return;
            }

            if (!IsMoving) return;
            
            Vector2 direction = (TargetPos - (Vector2)transform.position);
            
            Vector2 animatorVector = direction;
            animatorVector = Math.Abs(animatorVector.x) > Math.Abs(animatorVector.y) ?
                new Vector2(animatorVector.x, 0) :
                new Vector2(0, animatorVector.y);
            
            player.Animator.SetFloat(X, animatorVector.normalized.x);
            player.Animator.SetFloat(Y, animatorVector.normalized.y);

            if (direction.magnitude < 0.1f || beforePos == transform.position)
            {
                Arrive();
            }
            else
            {
                Rd.linearVelocity = direction.normalized * speed;
                beforePos = transform.position;
            }
        }

        protected virtual void MoveStart()
        {
            player.ChangeState(PlayerStateType.Move, (int)player.Animator.GetFloat(X), (int)player.Animator.GetFloat(Y));
        }

        protected virtual void Arrive()
        {
            beforePos = new Vector2(99999, 99999);
            Rd.linearVelocity = Vector2.zero;
            IsMoving = false;
            player.ChangeState(PlayerStateType.Idle, (int)player.Animator.GetFloat(X), (int)player.Animator.GetFloat(Y));
        }

        protected void SetSpeed(float set)
        {
            speed = set;
        }
    }
}
