using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using UnityEngine;

namespace _02Script.UI.Dialog.Do
{
    //오브젝트들을 삭제, 이 스크립트를 삭제
    public class DeleteGameObject :MonoBehaviour, IDialogCanScript
    {
        [SerializeField] private GameObject[] Objs;
        public virtual void Do(DoScriptType type)
        {
            if(type != DoScriptType.DeleteGameObject &&
               type != DoScriptType.DialogDeleteObj) return;
            for (int i = 0; i < Objs.Length; i++)
            {
                Destroy(Objs[i]);
            }
            Destroy(this);
        }
    }
}