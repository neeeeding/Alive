using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.Oven
{
    public class OvenMiniGame :MonoBehaviour
    {
        [SerializeField] private RectTransform baseBread;
        [SerializeField] private RectTransform moveBread;
        
        private readonly float _minYpos = -100;
        private readonly float _maxYpos = 320;
        private readonly float _lessY = 50f;
        private readonly float _addY = 25f;

        private float _curBaseYpos; // 기준의Y 위치
        private float _curMoveYpos; // 조종의 위치 (플레이)
        private float _breadDifference;

        private float _curTime;
        private float _setTime = 1;

        private bool _isGame;

        private void OnEnable()
        {
            SetBaseBread();
            MiniGameTimer.OnEndMiniGame += CheckScore;
        }

        private void OnDisable()
        {
            MiniGameTimer.OnEndMiniGame -= CheckScore;
        }

        private void SetBaseBread() //위치등 설정
        {
            _curBaseYpos = Random.Range(_minYpos, _maxYpos);
            _curMoveYpos = _curBaseYpos;
            
            Vector3 setPos = baseBread.position;
            setPos.y = _curBaseYpos;
            baseBread.position = setPos;
            
            setPos = moveBread.position;
            setPos.y = _curBaseYpos;
            moveBread.position = setPos;

            _curTime = 0;
            _breadDifference = 0;
            _isGame = true;
        }

        private void Update()
        {
            if(_isGame)
                CheckTime();
        }
        
        private void CheckBread() //빵 간의 차이 비교 (가장 큰)
        {
            _breadDifference = Math.Max(Math.Abs(_curBaseYpos - _curMoveYpos), _breadDifference);
        }

        private void CheckTime() //한 텀 (시간)
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _setTime)
            {
                _curTime = 0;
                LessBread();
            }
        }

        private void LessBread() //자동으로 빵 내려가기
        {
            _curMoveYpos -= _lessY; 
            _curMoveYpos = Mathf.Max(_curMoveYpos, _minYpos - (_lessY*5));
            moveBread.transform.DOMoveY(_curMoveYpos, 0.3f);
            CheckBread();
        }

        public void AddBread() //클릭하여 빵 올리기
        {
            _curMoveYpos += _addY;
            _curMoveYpos = Mathf.Min(_curMoveYpos, _maxYpos + (_lessY*5));
            moveBread.transform.DOMoveY(_curMoveYpos, 0.3f);
            CheckBread();
        }

        private void CheckScore()
        {
            _isGame = false;
            _breadDifference = Math.Abs(_breadDifference/_lessY);
            _breadDifference = Mathf.Clamp(_breadDifference, 1, 6) -1;
            gameObject.SetActive(false);
            FoodScore.OnEndMiniGame?.Invoke(Math.Max((5 - (int)_breadDifference), 1));
        }
    }
}