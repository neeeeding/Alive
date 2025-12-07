using System;
using _02Script.UI.Dialog.Etc;
using UnityEngine;

namespace _02Script.UI.Dialog.Entity
{
    public class PlayerDialogInput : MonoBehaviour
    {
        public static event Action<bool> OnChat;
        
        [SerializeField] private DialogInputSO dialogInput;

        private void Awake()
        {
            dialogInput.OnNext += DialogInput;
        }

        private void OnDestroy()
        {
            dialogInput.OnNext -= DialogInput;
        }

        private void DialogInput(bool b)
        {
            OnChat?.Invoke(b);
        }
    }
}