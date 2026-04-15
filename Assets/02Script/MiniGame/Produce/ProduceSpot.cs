using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Produce
{
    public class ProduceSpot : MiniGameObj
    {
        [SerializeField] protected Image spotImage;

        private ProduceScore _score;
        protected ProduceScoreType type;
        protected Color _coolColor = new Color(0.2498f, 0.7302f, 0.2065f);
        protected Color _rightColor = new Color(0.9935f, 0.7072f, 0.1723f);
        protected Color _hotColor = new Color(0.8391f, 0, 0.2513f);
        protected float _changeMin = 1.0f;
        protected float _changeMax = 1.6f;
        protected float _curTime;
        protected float _changeTime;

        #region Mouse
        public virtual void ClickSpot()
        {
            spawn.ObjListAdd(this);
        }
        #endregion
        protected virtual void OnEnable()
        {
            _curTime = 0;
            _changeTime = Random.Range(_changeMin,_changeMax);
            type = ProduceScoreType.Hot;
            SetType();
        }

        protected virtual void OnDisable()
        {
            _score.GetScore(type,1);
        }

        public virtual void SetSpot(ProduceScore score)
        {
            _score = score;
        }

        protected virtual void Update()
        {
            _curTime += Time.deltaTime;
            if(_changeTime <= _curTime)
                Change();
        }

        protected virtual void Change()
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

        protected virtual void SetType()
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