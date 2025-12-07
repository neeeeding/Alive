using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.UI.Dialog.Etc
{
    // [CreateAssetMenu(fileName = "DialogInputSO", menuName = "SO/Input/DialogInputSO", order = 0)]
    public class DialogInputSO : ScriptableObject
     //   , Controls.IDialogActions
    {
        public event Action<int> OnClickSelect;
        public event Action<bool> OnNext;
    
        private Controls _controls;
    //     
    //     private void OnEnable()
    //     {
    //         if (_controls == null)
    //         {
    //             _controls = new Controls();
    //             _controls.Dialog.SetCallbacks(this);
    //         }
    //         _controls.Dialog.Enable();
    //     }
    //     private void OnDisable()
    //     {
    //         _controls.Dialog.Disable();
    //     }
    //     
    //     public void OnClick(InputAction.CallbackContext context)
    //     {
    //         if (context.performed)
    //         {
    //             OnNext?.Invoke(
    //                 context.control.displayName.ToLower() == "f");
    //         }
    //     }
    //
    //     public void OnSelect(InputAction.CallbackContext context) //숫자
    //     {
    //         if (context.performed)
    //         {   
    //             OnClickSelect?.Invoke(int.Parse(context.control.displayName));
    //         }
    //     }
     }
}