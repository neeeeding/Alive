using System;
using TMPro;
using UnityEngine;

namespace _02Script.MiniGame.Food.RiceCooker
{
    public class RiceCookerScore : MonoBehaviour
    {
        public static Action OnEndMiniGame;
        
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
                OnEndMiniGame?.Invoke();
                gameObject.SetActive(false);
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