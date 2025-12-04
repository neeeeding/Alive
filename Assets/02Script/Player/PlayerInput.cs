using System;
using _02Script.Etc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.Player
{
    public class PlayerInput : Singleton<PlayerInput>, Controls.IPCActions
    {
        public static event Action<Vector2> OnMousePos;
        public static event Action<Vector2> OnMovePos;
        
        private Controls _controls;
        
        private bool canInput;
        
        private Vector2 moveValue;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.PC.SetCallbacks(this);
            }
            _controls.PC.Enable();
        }

        private void OnDisable()
        {
            _controls.PC.Disable();
        }
        
        
        void Update()
        {
            if (!canInput) return;

            if (moveValue != Vector2.zero)
            {
                OnMovePos?.Invoke(moveValue);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (canInput)
                moveValue = context.ReadValue<Vector2>();
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            
        }

        public void OnRun(InputAction.CallbackContext context)
        {
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if(context.performed && canInput)
                OnMousePos?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        
        //---------------------------------------------------------------------------------------------------------

        public void NoInput()
        {
            canInput = false;
        }

        public void CanInput()
        {
            canInput = true;
        }

        public bool CheckCanInput()
        {
            return canInput;
        }
    }
}