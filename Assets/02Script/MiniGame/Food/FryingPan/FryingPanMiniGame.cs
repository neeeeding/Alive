using System;
using _02Script.MiniGame.Produce;

namespace _02Script.MiniGame.Food.FryingPan
{
    public class FryingPanMiniGame : MiniGameObjSpawn
    {
        private int _minusScore;

        private void OnEnable()
        {
            _minusScore = 0;
        }

        private void OnDisable()
        {
            EndGame();
        }

        public void GetScore(ProduceScoreType type)
        {
            if (type == ProduceScoreType.Hot || type == ProduceScoreType.Cool)
            {
                _minusScore++;
            }
        }

        private void EndGame()
        {
            FoodScore.OnEndMiniGame?.Invoke(Math.Clamp(5 - (_minusScore/2),1,5));
        }
    }
}