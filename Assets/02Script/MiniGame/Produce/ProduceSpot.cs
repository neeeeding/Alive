using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Produce
{
    public class ProduceSpot : MiniGameObj
    {
        [SerializeField] private Image spotImage;

        private ProduceScore _score;
        private ProduceScoreType type;
        private readonly Color _coolColor = new Color(0.2498f, 0.7302f, 0.2065f);
        private readonly Color _rightColor = new Color(0.9935f, 0.7072f, 0.1723f);
        private readonly Color _hotColor = new Color(0.8391f, 0, 0.2513f);
        private readonly float _changeMin = 1.0f;
        private readonly float _changeMax = 1.6f;
        private float _curTime;
        private float _changeTime;

        #region Mouse

        public void ClickSpot()
        {
            spawn.ObjListAdd(this);
        }

        #endregion
        private void OnEnable()
        {
            _curTime = 0;
            _changeTime = Random.Range(_changeMin,_changeMax);
            type = ProduceScoreType.Hot;
            SetType();
        }

        private void OnDisable()
        {
            _score.GetScore(type,1);
        }

        public void SetSpot(ProduceScore score)
        {
            _score = score;
        }

        private void Update()
        {
            _curTime += Time.deltaTime;
            if(_changeTime <= _curTime)
                Change();
        }

        private void Change()
        {
            _curTime = 0;
            if (type == ProduceScoreType.Hot)
            {
                _changeTime = Random.Range(_changeMin,_changeMax);
                type = ProduceScoreType.Right;
                SetType();
                return;
            }
            if (type == ProduceScoreType.Right)
            {
                _changeTime = Random.Range(_changeMin,_changeMax);
                type = ProduceScoreType.Cool;
                SetType();
                return;
            }
            if (type == ProduceScoreType.Cool)
            {
                _curTime = 999;
                ClickSpot();
            }
        }

        private void SetType()
        {

            if (type == ProduceScoreType.Hot)
            {
                spotImage.color = _hotColor;
                return;
            }

            if (type == ProduceScoreType.Cool)
            {
                spotImage.color = _coolColor;
                return;
            }
            if (type == ProduceScoreType.Right)
            {
                spotImage.color = _rightColor;
                return;
            }
        }
    }
}