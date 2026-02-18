using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "HouseSO", menuName = "SO/GoHouse/Block/HouseSO")]
    public class HouseSO : BlockActionSO
    {
        public static Action<string,BlockActionSO> OnSuccess;
        
        public string SceneName;
        public override void DoBlockAction()
        {
            OnSuccess?.Invoke(SceneName,this);
        }
    }
}