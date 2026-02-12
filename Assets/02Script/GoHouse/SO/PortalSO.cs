using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "PortalSO", menuName = "SO/GoHouse/PortalSO")]
    public class PortalSO : BlockActionSO
    {
        public static Action<BlockActionSO> OnPortalEnter;
        public override void DoBlockAction()
        {
            OnPortalEnter?.Invoke(this);
        }
    }
}