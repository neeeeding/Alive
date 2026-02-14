using System;
using _02Script.DoTweenUI.Warring;
using DG.Tweening;
using UnityEngine;

namespace _02Script.GoHouse
{
    public class BlockPlayer : MonoBehaviour
    {
        [SerializeField] private MoveCountUI countUI;
        
        private BlockManager _blockManager;
        private RectTransform _myRect;
        private Vector2 _playerPos;
        private readonly float _moveSpeed = 1f;
        private int _canMove;
        private int _curMove;

        #region EnDiAw
        private void OnEnable()
        {
            InputBtn.OnMoveBtn += MovePlayer;
        }
        private void OnDisable()
        {
            InputBtn.OnMoveBtn -= MovePlayer;
        }
        private void Awake()
        {
            _myRect = gameObject.GetComponent<RectTransform>();
        }

        #endregion


        public void SetPlayerPos(Vector2 pos,int moveCount,BlockManager manager)
        {
            _playerPos = Vector2.zero;
            _canMove = moveCount;
            _curMove = -1; //처음에 움직이니까
            _blockManager = manager;
            MovePlayer(pos);
        }

        private void MovePlayer(Vector2 moveWant)
        {
            if (_curMove >= _canMove)
            {
                WarringManager.Warring.ShowWarring("이동 횟수를 초과했습니다.");
                //게임 초기화 및 5% 뺏기 (주석)
                return;
            }
            if(_blockManager == null) return;
            
            Vector2? movePos = _blockManager.WantPos(_playerPos + moveWant);
            
            if(movePos == null)
            {
                WarringManager.Warring.ShowWarring("이동 할 수 없습니다.");
                return;
            }

            _playerPos = new Vector2(_playerPos.x + moveWant.x, _playerPos.y + moveWant.y);
            _myRect.DOMove(movePos.Value,_moveSpeed); //이동
            _curMove++;
            countUI.CountText(_curMove,_canMove);
        }
    }
}