using _02Script.MiniGame.Produce;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.Pot
{
    public class PotBubbleSpawn :MiniGameObjSpawn
    {
        [SerializeField] private GameObject listonBtn;
        [SerializeField] private GameObject startBtn;
        
        private readonly int _minCount = 8;
        private readonly int _maxCount = 15;
        
        private int _setCount; //목표 수 
        private int _count; //현재 버블 수
        private bool _isPlay; //듣는게 아니라 게임 진행 중인지
        private bool _isBubble; //듣는 거든 뭐든 버블

        private void OnEnable()
        {
            listonBtn.SetActive(true);
            startBtn.SetActive(false);
            SetBubbleCount();
            minTime = 0.8f;
            maxTime = 1.5f;
        }

        private void SetBubbleCount()
        {
            _setCount = Random.Range(_minCount, _maxCount);
            _count = 0;
            _isBubble = false;
            _isPlay= false;
        }

        public void Liston() //미리 들어보기
        {
            listonBtn.SetActive(false);
            startBtn.SetActive(false);
            _isBubble = true;
            _count = 0;
        }

        public void Play()
        {
            listonBtn.SetActive(false);
            startBtn.SetActive(false);
            _isPlay = true;
            Liston();
        }

        protected override void NewObj() //듣기면 정해진 수만큼만, 플레이면 계속해서
        {
            if (_count >= _setCount && !_isPlay)
            {
                _isBubble = false;
                listonBtn.SetActive(true);
                startBtn.SetActive(true);
                return;
            }
            _count++;
            base.NewObj();
        }

        protected override void Update()
        {
            if (!_isBubble) return;
            base.Update();
        }

        public void CheckScore() // 뚜껑 누름 (게임 종료)
        {
            if(!_isPlay) return;
            _isBubble = false;
            _isPlay = false;

            _count = 5 - Mathf.Abs(_setCount - _count);
            FoodScore.OnEndMiniGame?.Invoke(Mathf.Max(_count, 1));
            gameObject.SetActive(false);
        }
    }
}