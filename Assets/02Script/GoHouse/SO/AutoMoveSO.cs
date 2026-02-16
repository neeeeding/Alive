using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "AutoMoveSO", menuName = "SO/GoHouse/Block/AutoMoveSO")]
    public class AutoMoveSO : BlockActionSO
    {
        public static Action<Vector2> OnMove;
        
        public Vector2 movePos;
        public override void DoBlockAction()
        {
            OnMove?.Invoke(movePos);
        }
    }
}