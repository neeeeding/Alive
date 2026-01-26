using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class RaeliaEvent : GameEvent, IDialogCanScript
    {
        public override void GetItem()
        {
            if(isLock) return;
            isLock = true;
            CurrItem = Random.Range(0, itemDataSos.Length);
            OnGetItem?.Invoke(itemDataSos[CurrItem], Random.Range(2, 6));
        }

        public void Do(DoScriptType type)
        {
            if(type!= DoScriptType.RaeliaEvent) return;
            GetItem();
        }
    }
}