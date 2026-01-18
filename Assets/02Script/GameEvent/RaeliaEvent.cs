using UnityEngine;

namespace _02Script.GameEvent
{
    public class RaeliaEvent : GameEvent
    {
        public override void GetItem()
        {
            if(isLock) return;
            isLock = true;
            CurrItem = Random.Range(0, itemDataSos.Length);
            OnGetItem?.Invoke(itemDataSos[CurrItem], Random.Range(2, 6));
        }
    }
}