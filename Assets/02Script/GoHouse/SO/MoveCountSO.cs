using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "MoveCountSO", menuName = "SO/GoHouse/Block/MoveCountSO")]
    public class MoveCountSO: BlockActionSO
    {
        public static Action<int> Move;
        
        public int moveCount; // 증가 혹은 감소할
        public override void DoBlockAction()
        {
            Move?.Invoke(moveCount); //최초 한번만 실행해야 하나?
        }
    }
}