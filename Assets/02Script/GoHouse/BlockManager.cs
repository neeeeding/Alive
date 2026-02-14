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

        public Vector2? WantPos(Vector2 pos)
        {
            if(!_blockPos.ContainsKey(pos) || _blockPos[pos].isWall) return null;
            //갈 수 있다면 엔터까지
            _blockPos[pos].EnterBlock();
            return _blockPos[pos].Pos();
        }

        private void Awake()
        {
            if(curStage) SetStage(curStage);
        }

        private void SpawnBlock()
        {
            for (int y = curStage.stageBlocks.Count -1; y >= 0; y--) //왼 아래가 0,0 되도록
            {
                for (int x = 0; x < curStage.stageBlocks[y].columns.Count; x++)
                {
                    BlockObj obj = Instantiate(blockPrefab,blockParent);
                    BlockSO so = curStage.stageBlocks[y].columns[x];
                    obj.SetBlockData(so, new Vector2(x,y));
                    _blockPos.Add(new Vector2(x,y), obj);
                    obj.gameObject.SetActive(true);
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
    }
}