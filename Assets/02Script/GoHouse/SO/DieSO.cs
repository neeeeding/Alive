using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "DieSO", menuName = "SO/GoHouse/Block/Block/DieSO")]
    public class DieSO : BlockActionSO
    //Die할 때 Less도 같이 있을 것.
    {
        public static Action OnDie;
        
        public override void DoBlockAction()
        {
            OnDie?.Invoke();
        }
    }
}