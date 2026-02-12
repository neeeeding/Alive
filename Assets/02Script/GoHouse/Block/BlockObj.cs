using _02Script.GoHouse.SO;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.GoHouse.Block
{
    public class BlockObj : MonoBehaviour
    {
        [SerializeField] private BlockSO blockSo;
        [SerializeField] private Image blockImage;
        [SerializeField] private GameObject fogImage;

        private RectTransform _myRect;
        private Vector2 _myPos;
        
        private Vector2 _blockPos;
        private bool _isFog; //안개인지
        private bool _isBreakdown; //붕괴될 수 있는지
        private bool _isBreak; //붕괴 됬는지

        public bool isWall; //못 가는지

        public void EnterBlock()
        {
            blockSo.BlockAction();
        }

        public Vector2 Pos()
        {
            return _blockPos;
        }

        private void Awake()
        {
            SetMy();
        }

        #region Set
        private void SetMy()
        {
            _myRect = gameObject.GetComponent<RectTransform>();
            _myPos = _myRect.anchoredPosition;
        }

        public void SetBlockData(BlockSO so,Vector2 pos,bool fog = false,bool breakdown = false)
        {
            blockSo = so;
            blockImage.sprite = so.blockImage;
            _blockPos = pos;
            _isFog = fog;
            _isBreakdown = breakdown;
            _isBreak = false;

            isWall = (so.blockType == BlockType.Wall 
                      || so.blockType == BlockType.LockRoom);

            fogImage.SetActive(_isFog);
        }
        #endregion
    }
}