using _02Script.MiniGame.Produce;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.MiniGame.Food.FryingPan
{
    public class FryingPanMeat : ProduceSpot
    {
        [Header("FryingPan")]
        [SerializeField]private GameObject meat;
        [SerializeField]private Image frontMeat;
        [SerializeField]private Image backMeat;
        
        private FryingPanMiniGame _score;
        private ProduceScoreType _front;
        private ProduceScoreType _back;
        private bool _isFront; //앞뒤 구별

        public override void ClickSpot()
        {
            if (!_isFront)
            {
                _back = type;
                _score.GetScore(_back);
                spawn.ObjListAdd(this);
                return;
            }
            _isFront = false;
            _front = type;
            //뒤집는 애니메이션??
            (frontMeat.color, backMeat.color) = (backMeat.color, frontMeat.color);
            _score.GetScore(_front);
        }

        protected override void OnDisable()
        {
        }

        public void SetSpot(FryingPanMiniGame score)
        {
            _score = score;
        }

        protected override void OnEnable()
        {
            _front = ProduceScoreType.None;
            _back = ProduceScoreType.None;
            type = ProduceScoreType.None;
            _isFront = true;
            
            _changeMin = 2f;
            _changeMax = 3f;
            _curTime = 0;
            _changeTime = Random.Range(_changeMin,_changeMax);
            spotImage = backMeat;
            SetType();
        }

        protected override void Change()
        {
            _curTime = 0;
            if (type == ProduceScoreType.Hot)
            {
                _curTime = 999;
                ClickSpot();
                return;
            }
            if (type == ProduceScoreType.Right)
            {
                _changeTime = Random.Range(_changeMin,_changeMax);
                type = ProduceScoreType.Hot;
                SetType();
                return;
            }
            if (type == ProduceScoreType.Cool)
            {
                _changeTime = Random.Range(_changeMin,_changeMax);
                type = ProduceScoreType.Right;
                SetType();
            }
        }
    }
}