using _02Script.Manager;
using _02Script.UI.Save;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Player.State
{
    public class PlayerMovement: Singleton<PlayerMovement>
    {
        public float speed; //속도
        [HideInInspector] public Vector2 TargetPos; //마우스 위치
        private Rigidbody2D _rigidbody;
        private bool _isMoving;

        #region endiaw

        private void Awake()
        {
            GameManager.OnStart += StartLoad;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _isMoving = false;
            PlayerMobileInput.mousePos += Move;
            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            GameManager.OnStart -= StartLoad;
            PlayerMobileInput.mousePos -= Move;
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

        private void Move(Vector2 mousePos)
        {
            _isMoving = true;
            TargetPos = mousePos;
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
    }
}
