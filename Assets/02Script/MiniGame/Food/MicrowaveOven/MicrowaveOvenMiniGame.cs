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
        
        private float _setTime;
        private float _curTime;
        private bool _isTimer;
        private int _score;

        private void OnEnable()
        {
            SetRandomTime();
        }

        public void StartTimer()
        {
            _isTimer = true;
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
            _setTime = Random.Range(_minTime, _maxTime);
            setTimeText.text = $"{_setTime} 초";
        }

        private void CheckScore()
        {
            _curTime =- _setTime;
            
            _score = (int)(Math.Abs(_curTime) / 0.1f);
            _score = Math.Clamp(5-(_score / 2), 1, 5);
            FoodScore.OnEndMiniGame?.Invoke(_score);
        }
    }
}