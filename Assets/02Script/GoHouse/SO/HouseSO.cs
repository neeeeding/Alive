using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "HouseSO", menuName = "SO/GoHouse/HouseSO")]
    public class HouseSO : BlockActionSO
    {
        public static Action<string, BlockActionSO> OnPortalEnter;
        
        public string SceneName;
        public override void DoBlockAction()
        {
            OnPortalEnter?.Invoke(SceneName, this);
        }
    }
}