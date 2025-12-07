using UnityEngine;

namespace _02Script.UI.Dialog.Etc
{
    public class DeleteGameObject : DialogCanScript, IDialogCanScript
    {
        [SerializeField] private GameObject[] Objs;
        public virtual void Do()
        {
            for (int i = 0; i < Objs.Length; i++)
            {
                Destroy(Objs[i]);
            }
            Destroy(this);
        }
    }
}