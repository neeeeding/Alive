using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.GameCamera
{
    public class WheelMoveCameraInput: MonoBehaviour, Controls.IPCActions
    {
        [SerializeField] private Camera myCamera;
        [SerializeField] private float cameraMoveSpeed = 4;
        [SerializeField] private float maxZoomSize = 10;
        
        private Controls _controls;
        
        private Vector2 _wheelValue;
        private Vector3? _baseWheelValue;
        private Vector3 _moveValue;
        private bool _isWheel;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.PC.SetCallbacks(this);
            }
            _controls.PC.Enable();
            _baseWheelValue = null;
            _isWheel = false;
        }

        private void OnDisable()
        {
            _controls.PC.Disable();
        }
        
        
        void Update()
        {
            WheelMove();
            
            if (!MousePos()) return;
            WheelSize();
        }

        #region keyInput
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!MousePos()) return;
            _moveValue = (Vector3)context.ReadValue<Vector2>().normalized / cameraMoveSpeed * 0.5f;
            _baseWheelValue = Vector3.one;
            
            if(context.canceled)
                _baseWheelValue = null;
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
        }

        public void OnRun(InputAction.CallbackContext context)
        {
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
        }

        public void OnMouseWheel(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                if (!MousePos()) return;
                _isWheel = true;
                _baseWheelValue = Input.mousePosition;
            }
            else if(context.canceled)
            {
                _isWheel = false;
                _baseWheelValue = null;
            }
        }
        #endregion

        #region wheel Do

        private void WheelMove()
        {
            if (!_baseWheelValue.HasValue) return;
            
            if(_isWheel)
            {
                _moveValue = (_baseWheelValue.Value -Input.mousePosition).normalized/ cameraMoveSpeed;
                _baseWheelValue = Input.mousePosition;
                myCamera.gameObject.transform.position += _moveValue;
            }
            else
            {
                myCamera.gameObject.transform.position += _moveValue;
            }
        }

        private void WheelSize()
        {
            _wheelValue = Input.mouseScrollDelta;
            if (_wheelValue == Vector2.zero) return;
            
            myCamera.orthographicSize += _wheelValue.y * -0.2f;
            if (myCamera.orthographicSize > maxZoomSize)
            {
                myCamera.orthographicSize = maxZoomSize;
            }
            if (myCamera.orthographicSize < 1)
            {
                myCamera.orthographicSize = 1;
            }
        }
        #endregion

        #region MouseEnter

        private bool MousePos()
        {
            Vector3 viewportPos = myCamera.ScreenToViewportPoint(Input.mousePosition);

            return (viewportPos.x >= 0f && viewportPos.x <= 1f &&
                    viewportPos.y >= 0f && viewportPos.y <= 1f);
        }
        #endregion
    }
}