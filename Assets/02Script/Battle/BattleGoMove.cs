using System;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.Battle
{
    public class BattleGoMove : MonoBehaviour
    {
        private readonly int[] autoX = { 10, 0, -10, 0 };
        private readonly int[] autoY = { 0, 10, 0, -10 };
        public float speed = 5; //속도
        [HideInInspector] public Vector2 TargetPos; //갈 위치
        
        private CancellationTokenSource cts = new(); //시간을 위해
        private Animator _animator;
        private Rigidbody2D _rd;
        private bool _isMoving;
        private Vector3 beforePos; //위치 확인용

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rd = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            beforePos = new  Vector2(99999,99999);
            _ = AutoMove();
        }

        private void FixedUpdate()
        {
            if(!_isMoving) return;
            
            Vector2 direction = (TargetPos - (Vector2)_rd.transform.position);
            
            if (direction.magnitude < 0.1f) // 너무 가깝거나 이동을 못하는 상태라면 멈추기
            {
                Arrive();
            }
            else if (beforePos == _rd.transform.position)
            {
                beforePos = new  Vector2(99999,99999);
                int auto = Random.Range(0, autoX.Length);
            
                _isMoving = true;
                 TargetPos = (Vector2)transform.position + new Vector2(autoX[auto], autoY[auto]);
                MoveStart();
                _rd.linearVelocity = direction.normalized * speed;
            }
            else
            {
                _rd.linearVelocity = direction.normalized * speed;
                beforePos = _rd.transform.position;
            }
        }
        
        private async Task AutoMove()
        {
            while (!cts.IsCancellationRequested)
            {
                if (!gameObject.activeSelf)
                {
                    await Task.Yield();
                    continue;
                }
                try
                {
                    int rI = Random.Range(0, 10);
                    if (rI == 0) //멈춰 있기
                    {
                        await AsyncTime.WaitSeconds(Random.Range(1,1.5f), cts.Token, false);
                        continue;
                    }
                    await AsyncTime.WaitSeconds(5, cts.Token, false); 
                    
                    int auto = Random.Range(0, autoX.Length);
            
                    _isMoving = true;
                    TargetPos = (Vector2)transform.position + new Vector2(autoX[auto], autoY[auto]);
                    MoveStart();
                }
                catch (TaskCanceledException){break;}
            }
        }

        private void MoveStart()
        {
            _animator.SetBool("IsMove", true);
        }

        private void Arrive()
        {
            beforePos = new  Vector2(99999,99999);
            _rd.linearVelocity = Vector2.zero;
            _isMoving = false;
            _animator.SetBool("IsMove", false);
        }
    }
}