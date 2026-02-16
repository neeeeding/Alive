using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "KeySO", menuName = "SO/GoHouse/Block/KeySO")]
    public class KeySO : BlockActionSO
    {
        public static Action<int> OnKey;
        
        public int roomNum;
        public override void DoBlockAction()
        {
            OnKey?.Invoke(roomNum);
        }
    }
}