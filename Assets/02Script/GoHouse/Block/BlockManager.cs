using System;
using System.Collections.Generic;
using _02Script.Etc;
using _02Script.GoHouse.SO;
using _02Script.GoHouse.Stage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.GoHouse.Block
{
    public class BlockManager : MonoBehaviour
    {
        [Header("Show")]
        [SerializeField]private GoHouseStageSO curStage;
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
            EnterBlock(pos);
            return _blockPos[pos].Pos();
        }

        private async void EnterBlock(Vector2 pos)
        {
            await AsyncTime.WaitSeconds(1f);
            _blockPos[pos].EnterBlock();
        }

        public void Skip()
        {
            _typeBlock[BlockType.House][0].EnterBlock();
        }

        #region EnDiAw
        private void OnEnable()
        {
            PortalSO.OnPortalEnter += Portal;
            DieSO.OnDie += Die;
            BlockPlayer.OnReSet += Die;
        }
        private void OnDisable()
        {
            PortalSO.OnPortalEnter -= Portal;
            DieSO.OnDie -= Die;
            BlockPlayer.OnReSet -= Die;
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
            
            for (int y = 0; y< curStage.stageBlocks.Count; y++) //왼 아래가 0,0 되도록
            {
                for (int x = 0; x < curStage.stageBlocks[y].columns.Count; x++)
                {
                    BlockObj obj = Instantiate(blockPrefab,blockParent);
                    BlockSO so = curStage.stageBlocks[y].columns[x];
                    obj.SetBlockData(new Vector2(x,curStage.stageBlocks.Count - y - 1),so);
                    _blockPos.Add(new Vector2(x,curStage.stageBlocks.Count - y - 1), obj);
                    obj.gameObject.SetActive(true);

                    if (!_typeBlock.ContainsKey(so.blockType))
                    {
                        _typeBlock.Add(so.blockType, new List<BlockObj>());
                    }
                    _typeBlock[so.blockType].Add(obj);
                }
            }
        }
        
        public async void SetStage(GoHouseStageSO so)
        {
            curStage = so;
            SpawnBlock();
            stageScreen.SetScreenSize(curStage.stageBlocks[0].columns.Count,curStage.stageBlocks.Count);
            await AsyncTime.WaitSeconds(1f,false); //로딩 되라고...
            player.SetPlayerPos(curStage.playerPos, so.moveCount,this);
        }
        #endregion

        #region BlockAction
        private void Die()
        {
            player.SetPlayerPos(curStage.playerPos, curStage.moveCount,this);
        }
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