using _02Script.GoHouse.SO;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.GoHouse.Block
{
    public class BlockObj : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField] private BlockSO blockSo;
        [Header("Need")]
        [SerializeField] private Image blockImage;
        [SerializeField] private GameObject breakImage;
        [SerializeField] private GameObject fogImage;

        private RectTransform _myRect;
        
        private Vector2 _blockPos;
        private bool _isFog; //안개인지
        private bool _isBreakdown; //붕괴될 수 있는지
        private bool _isBreak; //붕괴 됐는지

        public bool isWall; //못 가는지
        private bool isUseBlock; //블럭 기능 이미 사용 했는지

        public void EnterBlock()
        {
            if(isUseBlock) return;
            blockSo.BlockAction();
            isUseBlock = true;
        }

        public Vector2 Pos()
        {
            return _myRect.position;
        }

        private void Awake()
        {
            _myRect = gameObject.GetComponent<RectTransform>();
        }

        #region Set
        public void SetBlockData(BlockSO so,Vector2 pos,bool fog = false,bool breakdown = false)
        {
            blockSo = so;
            blockImage.sprite = so.blockImage;
            _blockPos = pos;
            _isFog = fog;
            _isBreakdown = breakdown;
            isUseBlock = false;
            _isBreak = false;

            isWall = (so.blockType == BlockType.Wall 
                      || so.blockType == BlockType.LockRoom);

            fogImage.SetActive(_isFog);
            breakImage.SetActive(false);
        }
        #endregion
    }
}