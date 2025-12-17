using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JYE._01Script.Inventory.Inventory
{
    public class InventoryInput : MonoBehaviour, Controls.IInventoryActions
    {
        public event Action OnTab;
        
        private Controls _controls;

        private void OnEnable()
        {
            if(_controls == null)
            {
                _controls = new Controls();
                _controls.Inventory.SetCallbacks(this);
            }
            _controls.Inventory.Enable();
        }

        private void OnDisable()
        {
            _controls.Inventory.Disable();
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnTab?.Invoke();
            }
        }
    }
}