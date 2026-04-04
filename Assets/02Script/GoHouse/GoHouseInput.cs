using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.GoHouse
{
    public class GoHouseInput : MonoBehaviour, Controls.IGoHouseActions
    {
        public static event Action<Vector2> OnMovePos;
        public static event Action OnSkipClick;
        
        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.GoHouse.SetCallbacks(this);
            }
            _controls.GoHouse.Enable();
        }

        private void OnDisable()
        {
            _controls.GoHouse.Disable();
        }
        public void OnSkip(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSkipClick?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnMovePos?.Invoke(context.ReadValue<Vector2>());
        }
    }
}