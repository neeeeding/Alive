using System;
using System.Collections.Generic;
using _02Script.GoHouse.SO;
using UnityEngine;

namespace _02Script.GoHouse.Stage
{
    [CreateAssetMenu(fileName = "StageSO", menuName = "SO/GoHouse/StageSO", order = 0)]
    [Serializable]
    public class GoHouseStageSO : ScriptableObject
    {
        public string stageName;
        public int stageNum;
        public int moveCount;
        public Vector2 playerPos;

        public List<Row> stageBlocks = new List<Row>();
    }

    [Serializable]
    public class Row
    {
        public List<BlockSO> columns = new List<BlockSO>();
    }
}