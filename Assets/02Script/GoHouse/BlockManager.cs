using System.Collections.Generic;
using _02Script.GoHouse.Block;
using _02Script.GoHouse.SO;
using UnityEngine;

namespace _02Script.GoHouse
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private BlockObj blockPrefab;
        
        private Dictionary<Vector2, BlockObj> _blockPos = new Dictionary<Vector2, BlockObj>();

        public Vector2? WantPos(Vector2 pos)
        {
            if(_blockPos.ContainsKey(pos) || _blockPos[pos].isWall) return null;
            //갈 수 있다면 엔터까지
            _blockPos[pos].EnterBlock();
            return _blockPos[pos].Pos();
        }

        private void Awake()
        {
            
        }

        private void SpawnBlock()
        {
            //맵 so에 따라 구성하도록. 안개와 붕괴도 포함할 것. (주석)
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    BlockObj obj = Instantiate<BlockObj>(blockPrefab);
                    BlockSO so = null;
                    obj.SetBlockData(so, new Vector2(i,j));
                    _blockPos.Add(new Vector2(i, j), obj);
                    obj.gameObject.SetActive(true);
                }
            }
        }
    }
}