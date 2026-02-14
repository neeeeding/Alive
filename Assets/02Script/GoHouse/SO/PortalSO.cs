using System;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "PortalSO", menuName = "SO/GoHouse/Block/PortalSO")]
    public class PortalSO : BlockActionSO
    {
        public static Action<int,BlockActionSO> OnPortalEnter;

        public int portalID;
        public override void DoBlockAction()
        {
            OnPortalEnter?.Invoke(portalID,this);
        }
    }
}