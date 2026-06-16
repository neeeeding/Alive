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
        private readonly float _oneFloor = 300;
        private readonly float _oneYSize = 80;
        
        private readonly float _Speed = 150;
        private int _index;
        private bool _isMove;
        private bool _isDown;
        private bool _isSuccess;
        private Vector3 _moveDir;
        private CheckRect _center;

        private void Update()
        {
            if (_isDown)
            {
                DownCheck();
                Down();
            }
            if (Input.GetMouseButtonDown(0) && Time.timeScale >= 1 && _isMove)
            {
                ClickCheck();
                return;
            }
            if (_isMove)
            {
                SideMove();
            }
        }

        private void OnDisable()
        {
            gameObject.SetActive(false);
        }

        private void ClickCheck() // 클릭
        {
            _isMove = false;
            _isDown = true;
            _isSuccess = false;
            _moveDir = Vector3.down * _Speed;
        }

        private void SideMove()
        {
            transform.DOMoveX(transform.position.x+_moveDir.x,1).SetUpdate(false);
            if (transform.position.x < _minXpos)
            {
                _moveDir = Vector3.right;
                _moveDir *= _Speed;
            }
            if (transform.position.x > _maxXpos)
            {
                _moveDir = Vector3.left;
                _moveDir *= _Speed;
            }
        }

        private void Down()
        {
            if ( _isSuccess && transform.position.y <= _oneFloor + (_index * _oneYSize))
            {
                _isDown = false;
                _isSuccess =  false;
                return;
            }
            if (!_isSuccess && transform.position.y <= -1000 )
            {
                _isDown = false;
                OnFall?.Invoke(true);
                return;
            }
            transform.DOMoveY(transform.position.y+_moveDir.y,1).SetUpdate(false);
        }

        private void DownCheck() //밑에 확인 하기
        {
            if (!_isSuccess&&_center.Check(ReturnRect()))
            {
                _isSuccess =  true;
                OnFall?.Invoke(false);
            }
        }

        public void Setting(Sprite sprite, CheckRect center, int  index)
        {
            _center = center;
            _index = index;
            
            _isMove = true;
            _isDown = false;
            
            int r = Random.Range(0, 2);
            _moveDir = r == 0 ? Vector3.right : Vector3.left;
            _moveDir *= _Speed;
            
            obj.sprite = sprite;
        }
    }
}