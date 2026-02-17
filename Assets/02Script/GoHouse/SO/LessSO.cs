using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "LessSO", menuName = "SO/GoHouse/Block/LessSO")]
    public class LessSO : BlockActionSO
    {
        public static Action OnLess;
        public override void DoBlockAction()
        {
            OnLess?.Invoke();
        }
    }
}