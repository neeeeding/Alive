using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using UnityEngine;

namespace _02Script.UI.Dialog.Do
{
    //자신을 삭제함 (필요 없을 것 같음) (주석)
    public class DialogDeleteObj : DeleteGameObject
    {
        [SerializeField] private DialogEntity owner;

        public void Do(DialogEntity entity, DoScriptType type)
        {
            if(type != DoScriptType.DialogDeleteObj) return;
            if (entity != owner) return;
            base.Do(type);
        }
    }
}