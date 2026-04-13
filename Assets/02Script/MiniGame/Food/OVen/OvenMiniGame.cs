using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.OVen
{
    public class OvenMiniGame : FoodScore
    {
        [SerializeField] private GameObject baseBread;
        [SerializeField] private GameObject moveBread;
        
        private readonly float _minYpos = 120;
        private readonly float _maxYpos = 1080 - 120;
        private readonly float _lessY = 0.1f;
        private readonly float _addY = 1f;

        private float _curBaseYpos; // 기준의Y 위치
        private float _curMoveYpos; // 조종의 위치 (플레이)
        private float _breadDifference;

        private float _curTime;
        private float _setTime = 1;

        private bool _isGame;

        private void OnEnable()
        {
            SetBaseBread();
        }

        private void SetBaseBread() //위치등 설정
        {
            _curBaseYpos = Random.Range(_minYpos, _maxYpos);
            _curMoveYpos = _curBaseYpos;
            
            Vector3 setPos = baseBread.transform.position;
            setPos.y = _curBaseYpos;
            baseBread.transform.position = setPos;
            
            setPos = moveBread.transform.position;
            setPos.y = _curBaseYpos;
            moveBread.transform.position = setPos;

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
            _breadDifference = Math.Max(_curBaseYpos - _curMoveYpos, _breadDifference);
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
            baseBread.transform.DOMoveY(_curMoveYpos, 0.3f);
            CheckBread();
        }

        public void AddBread() //클릭하여 빵 올리기
        {
            _curMoveYpos += _addY;
            baseBread.transform.DOMoveY(_curMoveYpos, 0.3f);
            CheckBread();
        }

        private void CheckScore()
        {
            _isGame = false;
            _breadDifference = Math.Abs(_breadDifference);
            _breadDifference = Mathf.Clamp(_breadDifference, 1, 6) -1;
            OnEndMiniGame?.Invoke(5 - (int)_breadDifference);
        }
    }
}