using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.Battle
{
    public class BattleInput: MonoBehaviour, Controls.IBattleActions
    {
        public event Action<Vector2> OnMoveInput;
        public event Action<Vector2?, bool> OnMouseWheelInput;
        public event Action<string> OnInventoryInput;
        public event Action<string> OnSkillInput;
        public event Action<string> OnWeaponInput;
        public event Action<string> OnFoodInput;
        
        private Controls _controls;
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Battle.SetCallbacks(this);
            }
            _controls.Battle.Enable();
        }

        private void OnDisable()
        {
            _controls.Battle.Disable();
        }
        
        #region keyInput
        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnMouseWheel(InputAction.CallbackContext context)
        {
            if(context.started)
                OnMouseWheelInput?.Invoke(Input.mousePosition, true);
            else if(context.canceled)
                OnMouseWheelInput?.Invoke(null, false);
        }
        public void OnInventory(InputAction.CallbackContext context)
        {
            if(context.started)
                OnInventoryInput?.Invoke(context.control.displayName);
        }
        public void OnSkill(InputAction.CallbackContext context)
        {
            if(context.started)
                OnSkillInput?.Invoke(context.control.displayName);
        }
        public void OnWeapon(InputAction.CallbackContext context)
        {
            if(context.started)
                OnWeaponInput?.Invoke(context.control.displayName);
        }
        public void OnFood(InputAction.CallbackContext context)
        {
            if(context.started)
                OnFoodInput?.Invoke(context.control.displayName);
        }
        #endregion
    }
}