using System;
using _02Script.MiniGame.Produce;

namespace _02Script.MiniGame.Food.FryingPan
{
    public class FryingPanMiniGame : MiniGameObjSpawn
    {
        private int _minusScore;
        private int _meatCount = 5;
        private int _backMeat;

        private void OnEnable()
        {
            _backMeat = 0;
            _minusScore = 0;
            for (int i = 0; i < _meatCount; i++)
            {
                NewObj();
                
            }
        }

        protected override void ObjSetting()
        {
            base.ObjSetting();
            (_spotList[0] as FryingPanMeat).SetSpot(this);
        }

        private void OnDisable()
        {
            EndGame();
        }

        protected override void Update()
        {
        }

        public void GetScore(ProduceScoreType type)
        {
            _backMeat++;
            if (type == ProduceScoreType.Hot || type == ProduceScoreType.Cool)
            {
                _minusScore++;
            }

            if (_backMeat >= _meatCount * 2)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            if (_backMeat < _meatCount * 2) return;
            _backMeat = 0;
            FoodScore.OnEndMiniGame?.Invoke(Math.Clamp(5 - (_minusScore/2),1,5));
            gameObject.SetActive(false);
        }
    }
}