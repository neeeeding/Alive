using System;
using _02Script.GamePlayer;
using _02Script.MiniGame.Food.FryingPan;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.None
{
    public class NoneMiniGameMoveObj : CheckRect
    {
        public static Action<bool> OnFall;
        
        [SerializeField] private Image obj;
        
        private readonly float _minXpos = 0;
        private readonly float _maxXpos = 1920;
        
        private readonly float _minSpeed = 3;
        private readonly float _maxSpeed = 8;
        private float _curTime;
        private bool _isMove;
        private bool _isDown;
        private Vector3 _moveDir;
        private CheckRect _center;

        private void OnEnable()
        {
            PlayerInput.OnMousePos += ClickCheck;
        }

        private void OnDisable()
        {
            PlayerInput.OnMousePos -= ClickCheck;
        }

        private void Update()
        {
            if (_isMove)
            {
                SideMove();
            }
            else if (_isDown)
            {
                Down();
                DownCheck();
            }
        }

        private void ClickCheck( Vector2 v)
        {
            ClickCheck();
        }
        private void ClickCheck() // 클릭
        {
            _isMove = false;
            _isDown = true;
        }

        private void SideMove()
        {
            obj.transform.DOMoveX(obj.transform.position.x+_moveDir.x,1).SetEase((Ease)Random.Range(0,36)).SetUpdate(true);
            if (obj.transform.position.x < _minXpos)
            {
                _curTime = Random.Range(_minSpeed, _maxSpeed);
                _moveDir = Vector3.right;
                _moveDir *= _curTime;
            }
            if (obj.transform.position.x > _maxXpos)
            {
                _curTime = Random.Range(_minSpeed, _maxSpeed);
                _moveDir = Vector3.left;
                _moveDir *= _curTime;
            }
        }

        private void Down()
        {
            if (_moveDir.y <= 0)
            {
                _moveDir = Vector3.down * _maxSpeed;
            }
            obj.transform.DOMoveY(obj.transform.position.y+_moveDir.y,1).SetEase((Ease)Random.Range(0,36)).SetUpdate(true);
        }

        private void DownCheck() //밑에 확인 하기
        {
            if (_center.Check(ReturnRect()))
            {
                _isMove = false;
                _isDown = false;
            }
        }

        public void Setting(Sprite sprite, CheckRect center)
        {
            _center = center;
            
            _isMove = true;
            _isDown = false;
            
            _curTime = Random.Range(_minSpeed, _maxSpeed);
            int r = Random.Range(0, 2);
            _moveDir = r == 0 ? Vector3.right : Vector3.left;
            _moveDir *= _curTime;
            
            obj.sprite = sprite;
        }
    }
}