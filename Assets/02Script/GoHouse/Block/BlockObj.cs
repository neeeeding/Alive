using _02Script.Etc;
using _02Script.GoHouse.SO;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        public Vector2 MyPos;
        private bool _isFog; //안개인지
        private bool _isBreakdown; //붕괴될 수 있는지
        private bool _isBreak; //붕괴 됐는지

        public bool isWall; //못 가는지
        private bool _isUseBlock; //블럭 기능 이미 사용 했는지

        public void EnterBlock()
        {
            if(_isUseBlock) return;
            blockSo.BlockAction();
            if(blockSo.blockType != BlockType.Die && blockSo.blockType != BlockType.Portal && blockSo.blockType != BlockType.AutoMove)
                _isUseBlock = true;
            print(EnumToString.Name(blockSo.blockType));
        }
        
        public BlockSO ReturnType()
        {
            return blockSo;
        }

        public Vector2 Pos()
        {
            return _myRect.position;
        }

        #region EnDiAw
        private void OnEnable()
        {
            KeySO.OnKey += KeyLockRoom;
            DieSO.OnDie += Die;
            BlockPlayer.OnReSet += Die;
        }
        private void OnDisable()
        {
            KeySO.OnKey -= KeyLockRoom;
            DieSO.OnDie -= Die;
            BlockPlayer.OnReSet -= Die;
        }
        private void Awake()
        {
            _myRect = gameObject.GetComponent<RectTransform>();
        }
        #endregion

        #region BlockAction
        private void KeyLockRoom(int key)
        {
            if(blockSo.blockType != BlockType.LockRoom) return;

            foreach (BlockActionSO room in blockSo.actions)
            {
                if (room as LockRoomSO != null &&
                    (room as LockRoomSO).KeyCheck(key))
                {
                    isWall = false;
                }
            }
        }
        private void Die()
        {
            SetBlockData(MyPos, blockSo, _isFog, _isBreakdown);
            _isUseBlock = false;
        }
        #endregion

        #region Set
        public void SetBlockData(Vector2 pos,BlockSO so,bool fog = false,bool breakdown = false)
        {
            MyPos = pos;
            blockSo = so;
            blockImage.sprite = so.blockImage;
            _isFog = fog;
            _isBreakdown = breakdown;
            _isUseBlock = false;
            _isBreak = false;

            isWall = (so.blockType == BlockType.Wall 
                      || so.blockType == BlockType.LockRoom);

            fogImage.SetActive(_isFog);
            breakImage.SetActive(false);
        }
        #endregion
    }
}