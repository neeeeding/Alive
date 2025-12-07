using _02Script.UI.Dialog.Entity;
using UnityEngine;

namespace _02Script.UI.Dialog.Etc
{
    public class DialogDeleteObj : DeleteGameObject
    {
        [SerializeField] private DialogEntity owner;

        public void Do(DialogEntity entity)
        {
            if (entity != owner) return;
            base.Do();
        }
    }
}