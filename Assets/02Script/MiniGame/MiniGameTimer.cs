using System;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.MiniGame
{
    public class MiniGameTimer : MonoBehaviour
    {
        public static Action OnEndMiniGame;
        
        [SerializeField] private float maxTime = 20;
        [SerializeField] private Slider timerSlider;

        private float _curTime;
        private bool _isEnd;

        private void OnEnable()
        {
            Time.timeScale = 1;
            _curTime = maxTime;
            _isEnd = false;
        }

        private void Update()
        {
            if(_isEnd) return;
            TimeCalculate();
        }

        private void TimeCalculate()
        {
            timerSlider.value = _curTime / maxTime;
            _curTime -= Time.deltaTime;

            if (_curTime <= 0)
            {
                OnEndMiniGame?.Invoke();
                _isEnd = true;
            }
        }
    }
}