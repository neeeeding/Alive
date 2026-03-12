using System;
using _02Script.Etc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.GamePlayer
{
    public class PlayerInput : Singleton<PlayerInput>, Controls.IHomeActions
    {
        public static Action<bool> OnRunClick;
        public static event Action<Vector2> OnMousePos;
        public static event Action<Vector2> OnMovePos;
        
        private Controls _controls;
        
        private bool _canInput;
        
        private Vector2 _moveValue;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Home.SetCallbacks(this);
            }
            _controls.Home.Enable();
        }

        private void OnDisable()
        {
            _controls.Home.Disable();
        }
        
        
        void Update()
        {
            //if (!canInput) return;

            if (_moveValue != Vector2.zero)
            {
                OnMovePos?.Invoke(_moveValue);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveValue = context.ReadValue<Vector2>();
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnRunClick?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnRunClick?.Invoke(false);
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if(context.performed && _canInput &&
               Input.mousePosition.x < 1920 && Input.mousePosition.x > 0 &&
               Input.mousePosition.y < 1080 && Input.mousePosition.y > 0)
                OnMousePos?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        public void OnMouseWheel(InputAction.CallbackContext context)
        {
        }

        public void NoInput()
        {
            _canInput = false;
        }

        public void CanInput()
        {
            _canInput = true;
        }

        public bool CheckCanInput()
        {
            return _canInput;
        }
    }
}