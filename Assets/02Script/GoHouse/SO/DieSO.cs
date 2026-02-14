using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "DieSO", menuName = "SO/GoHouse/Block/Block/DieSO")]
    public class DieSO : BlockActionSO
    {
        public static Action OnDie;
        
        public override void DoBlockAction()
        {
            OnDie?.Invoke();
        }
    }
}