using DG.Tweening;
using UnityEngine;

namespace _02Script.GoHouse
{
    public class BlockPlayer : MonoBehaviour
    {
        [SerializeField] private BlockManager blockManager;
        private RectTransform _myRect;
        private Vector2 _playerPos;
        private float moveSpeed = 1f;

        private void Awake()
        {
            _myRect = gameObject.GetComponent<RectTransform>();
        }

        public void SetPlayerPos(Vector2 pos)
        {
            _playerPos = pos;
        }

        private void MovePlayer(Vector2 moveWant)
        {
            Vector2? movePos = blockManager.WantPos(_playerPos + moveWant);
            
            if(movePos == null) return; //못감.

            _playerPos = new Vector2(_playerPos.x + moveWant.x, _playerPos.y + moveWant.y);
            _myRect.DOMove(movePos.Value,moveSpeed); //이동
        }
    }
}