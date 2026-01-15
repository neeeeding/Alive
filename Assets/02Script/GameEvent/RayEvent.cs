using _02Script.UI.Store;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class RayEvent : Store
    {
        protected override void SetCardIndex(bool isPay)
        {
            CardIndex = Random.Range(0, isPay ? payDataSos.Length :  sellDataSos.Length);
        }
    }
}