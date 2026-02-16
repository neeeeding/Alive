using System.Collections.Generic;
using _02Script.Etc;
using _02Script.GoHouse.Block;
using _02Script.GoHouse.SO;
using _02Script.GoHouse.Stage;
using UnityEngine;

namespace _02Script.GoHouse
{
    public class BlockManager : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField]private StageSO curStage;
        [Header("Need")]
        [SerializeField] private StageScreen stageScreen;
        [SerializeField] private BlockPlayer player;
        [SerializeField] private Transform blockParent;
        [SerializeField] private BlockObj blockPrefab;
        
        private Dictionary<Vector2, BlockObj> _blockPos = new Dictionary<Vector2, BlockObj>();
        private Dictionary<BlockType, List<BlockObj>> _typeBlock = new Dictionary<BlockType, List<BlockObj>>();

        public Vector2? WantPos(Vector2 pos)
        {
            if(!_blockPos.ContainsKey(pos) || _blockPos[pos].isWall) return null;
            //갈 수 있다면 엔터까지
            _blockPos[pos].EnterBlock();
            return _blockPos[pos].Pos();
        }

        #region EnDiAw
        private void OnEnable()
        {
            PortalSO.OnPortalEnter += Portal;
        }
        private void OnDisable()
        {
            PortalSO.OnPortalEnter -= Portal;
        }
        private void Awake()
        {
            if(curStage) SetStage(curStage);
        }
        #endregion

        #region Set
        private void SpawnBlock()
        {
            _blockPos.Clear();
            _typeBlock.Clear();
            
            for (int y = curStage.stageBlocks.Count -1; y >= 0; y--) //왼 아래가 0,0 되도록
            {
                for (int x = 0; x < curStage.stageBlocks[y].columns.Count; x++)
                {
                    BlockObj obj = Instantiate(blockPrefab,blockParent);
                    BlockSO so = curStage.stageBlocks[y].columns[x];
                    obj.SetBlockData(new Vector2(x,y),so);
                    _blockPos.Add(new Vector2(x,y), obj);
                    obj.gameObject.SetActive(true);

                    if (!_typeBlock.ContainsKey(so.blockType))
                    {
                        _typeBlock.Add(so.blockType, new List<BlockObj>());
                    }
                    _typeBlock[so.blockType].Add(obj);
                }
            }
        }
        
        public async void SetStage(StageSO so)
        {
            curStage = so;
            SpawnBlock();
            stageScreen.SetScreenSize(curStage.stageBlocks[0].columns.Count,curStage.stageBlocks.Count);
            await AsyncTime.WaitSeconds(1f,false); //로딩 되라고...
            player.SetPlayerPos(curStage.playerPos, so.moveCount,this);
        }
        #endregion

        #region BlockAction
        private void Portal(BlockActionSO so)
        {
            List<BlockObj> portals = _typeBlock[BlockType.Portal];
            int r = 0;
            
            do
            {
                r = Random.Range(0, portals.Count);
                
            } while (!portals[r].ReturnType().actions.Contains(so) &&
                     player.Portal(portals[r].MyPos));
        }
        #endregion
    }
}