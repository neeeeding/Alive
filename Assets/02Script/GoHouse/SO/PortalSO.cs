using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "PortalSO", menuName = "SO/GoHouse/Block/PortalSO")]
    public class PortalSO : BlockActionSO
    {
        public static Action<BlockActionSO> OnPortalEnter;

        public int portalID; //나보라고...
        public override void DoBlockAction()
        {
            OnPortalEnter?.Invoke(this);
        }
    }
}