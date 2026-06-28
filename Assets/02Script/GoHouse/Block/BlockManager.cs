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
        [SerializeField] private GoHouseStageSO curStage;
        [Header("Need")]
        [SerializeField] private StageScreen stageScreen;
        [SerializeField] private BlockPlayer player;
        [SerializeField] private Transform blockParent;
        [SerializeField] private BlockObj blockPrefab;
        
        private Dictionary<Vector2, BlockObj> _blockPos = new Dictionary<Vector2, BlockObj>();
        private Dictionary<BlockType, List<BlockObj>> _typeBlock = new Dictionary<BlockType, List<BlockObj>>();
        private bool _canSkip;
        
        private readonly string _goHouseSoSave = "battle_GoHouseStageSoSave";

        public Vector2? WantPos(Vector2 pos)
        {
            if (!_blockPos.ContainsKey(pos) || _blockPos[pos].isWall) return null;
            EnterBlock(pos);
            return _blockPos[pos].Pos();
        }

        private async void EnterBlock(Vector2 pos)
        {
            await AsyncTime.WaitSeconds(1f);
            if (_blockPos.ContainsKey(pos))
                _blockPos[pos].EnterBlock();
        }

        public void Skip()
        {
            if (_canSkip && _typeBlock.ContainsKey(BlockType.House) && _typeBlock[BlockType.House].Count > 0)
                _typeBlock[BlockType.House][0].EnterBlock();
        }

        #region EnDiAw
        private void OnEnable()
        {
            _canSkip = false;
            SetStage(curStage);
            PortalSO.OnPortalEnter += Portal;
            DieSO.OnDie += Die;
            BlockPlayer.OnReSet += Die;
            GoHouseInput.OnSkipClick += Skip;
        }
        private void OnDisable()
        {
            PortalSO.OnPortalEnter -= Portal;
            DieSO.OnDie -= Die;
            BlockPlayer.OnReSet -= Die;
            GoHouseInput.OnSkipClick -= Skip;
        }
        #endregion

        private void LoadStage()
        {
            string json = PlayerPrefs.GetString(_goHouseSoSave);

            curStage = ScriptableObject.CreateInstance<GoHouseStageSO>();
            JsonUtility.FromJsonOverwrite(json, curStage);
            SetStage(curStage);
        }

        #region Set
        private void SpawnBlock()
        {
            _blockPos.Clear();
            _typeBlock.Clear();
            
            for (int y = 0; y < curStage.stageBlocks.Count; y++)
            {
                for (int x = 0; x < curStage.stageBlocks[y].columns.Count; x++)
                {
                    BlockObj obj = Instantiate(blockPrefab, blockParent);
                    BlockSO so = curStage.stageBlocks[y].columns[x];
                    obj.SetBlockData(new Vector2(x, curStage.stageBlocks.Count - y - 1), so);
                    _blockPos.Add(new Vector2(x, curStage.stageBlocks.Count - y - 1), obj);
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
            stageScreen.SetScreenSize(curStage.stageBlocks[0].columns.Count, curStage.stageBlocks.Count);
            await AsyncTime.WaitSeconds(1f, false);
            player.SetPlayerPos(curStage.playerPos, so.moveCount, this);
        }
        #endregion

        #region BlockAction
        private void Die()
        {
            player.SetPlayerPos(curStage.playerPos, curStage.moveCount, this);
            _canSkip = true;
        }

        // [수정] 무한루프 위험 제거 및 안전한 포탈 대상 선택
        private void Portal(BlockActionSO so)
        {
            if (!_typeBlock.ContainsKey(BlockType.Portal) || _typeBlock[BlockType.Portal].Count <= 0) return;
            List<BlockObj> portals = _typeBlock[BlockType.Portal];
            
            List<BlockObj> candidatePortals = portals.FindAll(p => !p.ReturnType().actions.Contains(so));
            if (candidatePortals.Count == 0)
            {
                candidatePortals = portals;
            }

            int r = Random.Range(0, candidatePortals.Count);
            player.Portal(candidatePortals[r].MyPos);
        }
        #endregion
    }
}
