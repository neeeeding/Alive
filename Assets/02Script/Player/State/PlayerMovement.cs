using System;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Manager;
using _02Script.UI.Save;
using _02Script.Etc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.Player.State
{
    public class PlayerMovement: Singleton<PlayerMovement>
    {
        public float speed; //속도
        [HideInInspector] public Vector2 TargetPos; //마우스 위치
        private Rigidbody2D _rigidbody;
        private bool _isMoving;
        
        private Player _player;
        private int[] autoX = { 1, 0, -1, 0 };
        private int[] autoY = { 0, 1, 0, -1 };
        
        protected CancellationTokenSource cts = new(); //시간을 위해

        #region endiaw

        private void Awake()
        {
            GameManager.OnStart += StartLoad;
            _rigidbody = GetComponent<Rigidbody2D>();
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _ = AutoMove();
        }

        private void OnEnable()
        {
            _isMoving = false;
            PlayerInput.OnMousePos += MouseMove;
            PlayerInput.OnMovePos += KeyboardMove;
            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= StartLoad;
            PlayerInput.OnMousePos -= MouseMove;
            PlayerInput.OnMovePos -= KeyboardMove;
            LoadCard.OnLoad -= Load;
        }
        #endregion

        private void FixedUpdate()
        {
            Vector2 direction = (TargetPos - (Vector2)transform.position);

            if (direction.magnitude < 0.1f || !_isMoving) // 너무 가까우면 멈추기
            {
                GameManager.Instance.PlayerStat.playerPosition = transform.position; //위치 저장
                _rigidbody.linearVelocity = Vector2.zero;
                _isMoving = false;
            }
            else
            {
                _rigidbody.linearVelocity = direction.normalized * speed;
            }
        }

        private async Task AutoMove()
        {
            while (!cts.IsCancellationRequested)
            {
                if (_player.isCurPlayer)
                {
                    await Task.Yield();
                    continue;
                }
                try
                {
                    await AsyncTime.WaitSeconds(Random.Range(0,1.1f), cts.Token);
                    
                    int auto = Random.Range(0, autoX.Length);
            
                    _isMoving = true;
                    TargetPos = (Vector2)transform.position + new Vector2(autoX[auto], autoY[auto]);
                    print(TargetPos);
                }
                catch (TaskCanceledException){break;}
            }
        }

        private void MouseMove(Vector2 mousePos)
        {
            if(!_player.isCurPlayer) return;
            _isMoving = true;
            TargetPos = mousePos;
        }
        private void KeyboardMove(Vector2 mousePos)
        {
            if(!_player.isCurPlayer) return;
            _isMoving = true;
            TargetPos = (Vector2)transform.position + mousePos.normalized;
        }

        private void StartLoad()
        {
            Vector2 position = GameManager.Instance.saveData.stat.playerPosition;
            GameManager.Instance.PlayerStat.playerPosition = position;
            Load();
        }

        private void Load()
        {
            transform.position = GameManager.Instance.PlayerStat.playerPosition;
        }
        
        protected virtual void OnDestroy()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}
