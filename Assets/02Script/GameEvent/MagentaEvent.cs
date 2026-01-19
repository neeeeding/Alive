using UnityEngine;

namespace _02Script.GameEvent
{
    public class MagentaEvent : GameEvent
    {
        public override void GetItem()
        {
            if(isLock) return;
            isLock = true;
            CurrItem = Random.Range(0, itemDataSos.Length);
            OnGetItem?.Invoke(itemDataSos[CurrItem], Random.Range(3, 9));
        }
    }
}