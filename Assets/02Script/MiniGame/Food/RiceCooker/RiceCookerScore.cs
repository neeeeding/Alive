using System;
using TMPro;
using UnityEngine;

namespace _02Script.MiniGame.Food.RiceCooker
{
    public class RiceCookerScore :MonoBehaviour
    {
        [SerializeField] private GameObject miniGame;
        [SerializeField] private TextMeshProUGUI scoreText;

        private readonly int _maxObjCount = 10;
        private int _endWorm;
        private int _worm;
        private int _rice;

        private void OnEnable()
        {
            SetScore();
            Text();
        }

        public void GetMinusScore()
        {
            _rice++;
            Text();
        }
        public void GetPlusScore()
        {
            _worm++;
            Text();
        }

        public void FindWorm()
        {
            _endWorm++;
            if (_endWorm >= _maxObjCount)
            {
                miniGame.SetActive(false);
                FoodScore.OnEndMiniGame?.Invoke(Math.Max(1,5 - (_rice/(_maxObjCount / 5))));
            }
            Text();
        }

        private void SetScore()
        {
            _endWorm = 0;
            _rice = 0;
            _worm = 0;
        }

        private void Text()
        {
            scoreText.text =
                $"남은 이물질 : {_maxObjCount - _endWorm}\n잡은 이물질 : {_worm} / {_maxObjCount}\n잘 못 잡은 쌀 : {_rice- (_endWorm - _worm)}";
        }
    }
}