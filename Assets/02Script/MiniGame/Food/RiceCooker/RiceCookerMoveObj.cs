using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.RiceCooker
{
    public class RiceCookerMoveObj : MonoBehaviour
    {
        [SerializeField] private RiceCookerMoveObjSpawn spawn;
        [SerializeField] private RiceCookerScore score;
        [SerializeField] private Image obj;
        [SerializeField]private float moveLength = 30;
        [SerializeField]private float moveSpeed = 1f;
        
        private readonly float _minXpos = 0;
        private readonly float _maxXpos = 1920;
        private readonly float _minLife = 3;
        private readonly float _maxLife = 5;

        private float _curLifeTime;
        private Vector3 _moveDir;
        private RiceCookerObjType _objType;
        private int _clickCount;

        #region Mouse
        public void ClickObj() // 잡기
        {
            _clickCount--;
            if (_clickCount != 0) return;

            if (_objType == RiceCookerObjType.Rice || _objType == RiceCookerObjType.Bean)
            {
                score.GetMinusScore();
            }
            Hide();
            
        }
        #endregion
        private void OnEnable()
        {
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (_curLifeTime <= 0) return;
            _curLifeTime -= Time.deltaTime;
            Move();
        }

        private void Move() //정해진 방향으로 움직이다, 끝에 도달하면 턴
        {
            if (_curLifeTime <= 0)
            {
                Hide();
                return;
            }
            
            obj.transform.DOMoveX(obj.transform.position.x+_moveDir.x,moveSpeed).SetEase((Ease)Random.Range(0,36)).SetUpdate(false);
             if (obj.transform.position.x < _minXpos)
             {
                 _moveDir = Vector3.right;
                 _moveDir *= moveLength;
             }
             if (obj.transform.position.x > _maxXpos)
             {
                 _moveDir = Vector3.left;
                 _moveDir *= moveLength;
             }
        }

        private void Hide() //페이드 아웃
        {
            //놓치면 알아서 감점 처리 & 현재 벌레및 나뭇가지 얼마나 나왔는지 측정
            if (_objType == RiceCookerObjType.Worm || _objType == RiceCookerObjType.Tree)
            {
                if (_clickCount > 0)
                {
                    score.GetMinusScore();
                }
                else
                {
                    score.GetPlusScore();
                }
                score.FindWorm();
            }
            
            obj.DOFade(0, 1).OnComplete(() =>
            {
                spawn.MoveObjListAdd(this); // 페이드 완료 후 반환
            });
        }

        public void SetObj(RiceCookerMoveObjSpawn sp, RiceCookerScore sc)
        {
            spawn = sp;
            score = sc;
        }
        public void Setting(Sprite sprite, RiceCookerObjType type) //필요한 세팅
        {
            obj.sprite = sprite;
            obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, 1);
            
            _objType = type;
            _clickCount = type == RiceCookerObjType.Worm ? Random.Range(2, 5) :
                type == RiceCookerObjType.Tree ? Random.Range(4, 7) : 1;
            _curLifeTime = Random.Range(_minLife,_maxLife);
            
            int r = Random.Range(0, 2);
            _moveDir = r == 0 ? Vector3.right : Vector3.left;
            _moveDir *= moveLength;
        }
    }

    public enum RiceCookerObjType
    {
        None = 0,
        Rice,
        Bean,
        Worm,
        Tree,
    }
}