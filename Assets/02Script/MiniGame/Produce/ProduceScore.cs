using _02Script.Produce.Weapon;
using TMPro;
using UnityEngine;

namespace _02Script.MiniGame.Produce
{
    public class ProduceScore : MonoBehaviour
    {
        [SerializeField] private ProduceResult result;
        [SerializeField] private TextMeshProUGUI coolText;
        [SerializeField] private TextMeshProUGUI rightText;
        [SerializeField] private TextMeshProUGUI hotText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private int _cool;
        private int _right;
        private int _hot;

        private void OnEnable()
        {
            SetScore();
        }

        private void OnDisable()
        {
            EndGame();
        }

        public void GetScore(ProduceScoreType scoreType, int add = 1)
        {
            switch (scoreType)
            {
                case ProduceScoreType.Cool :
                    _cool += add;
                    coolText.text = "식음 : " + _cool;
                    break;
                case ProduceScoreType.Right:
                    _right += add;
                    rightText.text = "적당 : " + _right;
                    break;
                case ProduceScoreType.Hot:
                    _hot += add;
                    hotText.text = "녹음 : " + _hot;
                    break;
                default:
                    break;
            }
        }
        
        private void EndGame()
        {
            int all = _cool + _right + _hot;
            _cool = (100 / all) * _cool;
            _right = _right / 2;
            _hot = _hot / 3;
            result.Score(_cool, _right, _hot);
            scoreText.text = coolText.text + $" -> 내구도 {_cool} 감소\n" +
                             rightText.text + $" -> 타격 {_right} 증가\n" +
                             hotText.text + $" -> 쿨타임 {_hot} 증가\n" +
                             ""; //점수 계산을 어떻게 할까??
        }

        private void SetScore()
        {
            _cool = 0;
            _right = 0;
            _hot = 0;
            GetScore(ProduceScoreType.Cool, 0);
            GetScore(ProduceScoreType.Right, 0);
            GetScore(ProduceScoreType.Hot, 0);
        }
    }
    
    public enum ProduceScoreType
    {
        None = 0,
        Cool = 1,
        Right,
        Hot,
        all,
    }
}