using UnityEngine;
using UnityEngine.InputSystem;

namespace _02Script.GameCamera
{
    public class WheelMoveCameraInput: MonoBehaviour, Controls.IPCActions
    {
        [SerializeField] protected Camera myCamera;
        [SerializeField] protected float cameraMoveSpeed = 4;
        [SerializeField] protected float maxZoomSize = 10;
        
        private Controls _controls;
        
        protected Vector2 wheelValue;
        protected Vector3? baseWheelValue;
        protected Vector3 moveValue;
        protected bool isWheel;
        protected bool isActive;

        protected virtual void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.PC.SetCallbacks(this);
            }
            _controls.PC.Enable();
            baseWheelValue = null;
            isWheel = false;
        }

        protected virtual void OnDisable()
        {
            _controls.PC.Disable();
        }
        
        private void Update()
        {
            if(!isActive) return;
            WheelMove();
            
            if (!MousePos()) return;
            WheelSize();
        }

        public void WheelStop(bool wheel)
        {
            isActive = wheel;
        }

        #region keyInput
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!MousePos()) return;
            moveValue = (Vector3)context.ReadValue<Vector2>().normalized / cameraMoveSpeed * 0.5f;
            baseWheelValue = Vector3.one;
            
            if(context.canceled)
                baseWheelValue = null;
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
                isWheel = true;
                baseWheelValue = Input.mousePosition;
            }
            else if(context.canceled)
            {
                isWheel = false;
                baseWheelValue = null;
            }
        }
        #endregion

        #region wheel Do

        protected virtual void WheelMove()
        {
            if (!baseWheelValue.HasValue) return;
            
            if(isWheel)
            {
                moveValue = (baseWheelValue.Value -Input.mousePosition).normalized/ cameraMoveSpeed;
                baseWheelValue = Input.mousePosition;
                myCamera.gameObject.transform.position += moveValue;
            }
            else
            {
                myCamera.gameObject.transform.position -= moveValue;
            }
        }

        protected virtual void WheelSize()
        {
            wheelValue = Input.mouseScrollDelta;
            if (wheelValue == Vector2.zero) return;
            
            myCamera.orthographicSize += wheelValue.y * -0.2f;
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

        protected virtual bool MousePos()
        {
            Vector3 viewportPos = myCamera.ScreenToViewportPoint(Input.mousePosition);

            return (viewportPos.x >= 0f && viewportPos.x <= 1f &&
                    viewportPos.y >= 0f && viewportPos.y <= 1f);
        }
        #endregion
    }
}