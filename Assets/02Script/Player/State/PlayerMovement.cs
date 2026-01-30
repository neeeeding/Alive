using System;
using _02Script.UI.InGame;
using UnityEngine;

namespace _02Script.Player.State
{
    public class PlayerMovement: MonoBehaviour
    {
        public float speed; //속도
        [HideInInspector] public Vector2 TargetPos; //갈 위치
        protected Rigidbody2D _rigidbody;
        protected bool _isMoving;
        
        protected Player _player;

        protected readonly string X = "X";
        protected readonly string Y = "Y";

        #region endiaw

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _player = GetComponent<Player>();
        }

        protected virtual void OnEnable()
        {
            _isMoving = false;
            RunBtn.OnMoveSpeed += SetSpeed;
        }

        protected virtual void OnDisable()
        {
            RunBtn.OnMoveSpeed -= SetSpeed;
        }
        #endregion

        protected virtual void FixedUpdate()
        {
            if(!_isMoving) return;
            
            Vector2 direction = (TargetPos - (Vector2)transform.position);
            
            Vector2 animatorVector = direction;
            animatorVector = Math.Abs(animatorVector.x) > Math.Abs(animatorVector.y)?
                new Vector2(animatorVector.x, 0):
                new Vector2(0, animatorVector.y);
            
            _player.Animator.SetFloat(X, animatorVector.normalized.x);
            _player.Animator.SetFloat(Y, animatorVector.normalized.y);

            if (direction.magnitude < 0.1f) // 너무 가까우면 멈추기
            {
                Arrive();
            }
            else
            {
                _rigidbody.linearVelocity = direction.normalized * speed;
            }
        }

        protected virtual void Arrive()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _isMoving = false;
        }

        protected void SetSpeed(float set)
        {
            speed = set;
        }
    }
}
