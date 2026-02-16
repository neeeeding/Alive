using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "MoveCountSO", menuName = "SO/GoHouse/Block/MoveCountSO")]
    public class MoveCountSO: BlockActionSO
    {
        public static Action<int> OnMove;
        
        public int moveCount; // 증가 혹은 감소할
        public override void DoBlockAction()
        {
            OnMove?.Invoke(moveCount);
        }
    }
}