using System.Collections.Generic;
using _02Script.GoHouse.Block;
using UnityEngine;

namespace _02Script.GoHouse
{
    public class BlockManager : MonoBehaviour
    {
        private Dictionary<Vector2, BlockObj> _blockPos = new Dictionary<Vector2, BlockObj>();

        public Vector2? WantPos(Vector2 pos)
        {
            if(_blockPos.ContainsKey(pos) || _blockPos[pos].isWall) return null;
            return _blockPos[pos].Pos();
        }
    }
}