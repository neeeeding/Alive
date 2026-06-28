using System;
using _02Script.DoTweenUI.Warring;
using _02Script.GoHouse.SO;
using _02Script.GoHouse.UI;
using DG.Tweening;
using UnityEngine;

namespace _02Script.GoHouse.Block
{
    public class BlockPlayer : MonoBehaviour
    {
        public static Action OnReSet;
        
        [SerializeField] private MoveCountUI countUI;
        
        private BlockManager _blockManager;
        private RectTransform _myRect;
        private Vector2 _playerPos;
        private readonly float _moveSpeed = 1f;
        private int _canMove;
        private int _curMove;

        private bool _isSuccess;

        #region EnDiAw
        private void OnEnable()
        {
            InputBtn.OnMoveBtn += BaseMove;
            AutoMoveSO.OnMove += AutoMove;
            HouseSO.OnSuccess += House;
            MoveCountSO.OnMove += MoveCount;
            GoHouseInput.OnMovePos += BaseMove;
        }
        private void OnDisable()
        {
            InputBtn.OnMoveBtn -= BaseMove;
            AutoMoveSO.OnMove -= AutoMove;
            HouseSO.OnSuccess -= House;
            MoveCountSO.OnMove -= MoveCount;
            GoHouseInput.OnMovePos -= BaseMove;
        }
        private void Awake()
        {
            _myRect = gameObject.GetComponent<RectTransform>();
        }

        #endregion

        public void SetPlayerPos(Vector2 pos, int moveCount, BlockManager manager)
        {
            _isSuccess = false;
            _playerPos = Vector2.zero;
            _canMove = moveCount;
            _curMove = 0;
            _blockManager = manager;
            Vector2? value = MoveCheck(pos);
            if (value != null)
            {
                TeleportMove(value.Value);
                SetPlayerPos(pos);
                MoveCount(0);
            }
        }

        #region Move
        private void BaseMove(Vector2 moveWant)
        {
            if (_isSuccess) return;
            Vector2? value = MoveCheck(moveWant);
            if (value != null)
            {
                MoveCount();
                DoTweenMove(value.Value);
                SetPlayerPos(moveWant);
            }
        }

        private Vector2? MoveCheck(Vector2 moveWant)
        {
            if (_blockManager == null || _isSuccess) return null;
            if (_curMove >= _canMove)
            {
                WarringManager.Warring.ShowWarring("이동 횟수를 초과했습니다.");
                OnReSet?.Invoke();
                return null;
            }
            
            Vector2? movePos = _blockManager.WantPos(_playerPos + moveWant);
            
            if (movePos == null)
            {
                WarringManager.Warring.ShowWarring("이동 할 수 없습니다.");
                return null;
            }
            return movePos;
        }
        
        private void DoTweenMove(Vector2 movePos)
        {
            _myRect.DOMove(movePos, _moveSpeed);
        }
        private void TeleportMove(Vector2 movePos)
        {
            _myRect.position = movePos;
        }
        private void MoveCount(int add = 1)
        {
            _curMove += add;
            countUI.CountText(_curMove, _canMove);
        }
        private void SetPlayerPos(Vector2 pos)
        {
            _playerPos = new Vector2(_playerPos.x + pos.x, _playerPos.y + pos.y);
        }
        #endregion

        #region BlockAction
        private void AutoMove(Vector2 moveWant)
        {
            Vector2? value = MoveCheck(moveWant);
            
            if (value != null)
            {
                DoTweenMove(value.Value);
                SetPlayerPos(moveWant);
            }
        }

        // [수정] 절대 좌표 직접 설정 및 정상 타일 위치로 텔레포트
        public bool Portal(Vector2 pos)
        {
            if (_playerPos == pos) return false;
            _playerPos = pos;
            Vector2? targetPos = _blockManager.WantPos(pos);
            if (targetPos != null)
            {
                TeleportMove(targetPos.Value);
                return true;
            }
            return false;
        }
        private void House(string sceneName, BlockActionSO _)
        {
            _isSuccess = true;
        }
        #endregion
    }
}
