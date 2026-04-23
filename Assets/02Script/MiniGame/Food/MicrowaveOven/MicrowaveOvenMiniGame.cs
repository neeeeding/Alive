using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.MicrowaveOven
{
    public class MicrowaveOvenMiniGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI setTimeText;
        
        private readonly float _minTime = 5;
        private readonly float _maxTime = 10;
        
        private int _setTime;
        private float _curTime;
        private bool _isTimer;
        private int _score;

        private void OnEnable()
        {
            _isTimer = false;
            SetRandomTime();
        }

        public void Btn()
        {
            if (_isTimer)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            _isTimer = true;
            _curTime = 0;
        }
        public void StopTimer()
        {
            _isTimer = false;
            CheckScore();
        }

        private void Update()
        {
            if (!_isTimer) return;
            _curTime += Time.deltaTime;
        }

        private void SetRandomTime()
        {
            _setTime = (int)Random.Range(_minTime, _maxTime);
            setTimeText.text = $"{_setTime} 초";
        }

        private void CheckScore()
        {
            _curTime -= _setTime;
            
            _score = (int)Math.Abs(_curTime / 0.2f);
            _score = Math.Clamp(5-(_score / 2), 1, 5);
            FoodScore.OnEndMiniGame?.Invoke(_score);
            gameObject.SetActive(false);
        }
    }
}