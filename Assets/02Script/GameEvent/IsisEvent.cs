using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class IsisEvent : GameEvent, IDialogCanScript
    {
        public void Do(DoScriptType type)
        {
            if(type != DoScriptType.IsisEvent) return;
            GetItem();
        }
    }
}