using _02Script.Battle;
using UnityEngine;

namespace _02Script.GameCamera
{
    public class WheelMoveCameraInput: MonoBehaviour
    {
        [SerializeField] protected Camera myCamera;
        [SerializeField] protected float cameraMoveSpeed = 4;
        [SerializeField] protected float maxZoomSize = 10;
        [SerializeField] protected BattleInput battleInput;
        
        protected Vector2 wheelValue;
        protected Vector3? baseWheelValue;
        protected Vector3 moveValue;
        protected bool isWheel;
        protected bool isActive;

        protected virtual void OnEnable()
        {
            battleInput.OnMoveInput += OnMove;
            battleInput.OnMouseWheelInput += OnMouseWheel;
            
            baseWheelValue = null;
            isWheel = false;
        }

        protected virtual void OnDisable()
        {
            battleInput.OnMoveInput -= OnMove;
            battleInput.OnMouseWheelInput -= OnMouseWheel;
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
        public void OnMove(Vector2 input)
        {
            if (!MousePos()) return;
            moveValue = (Vector3)input.normalized / cameraMoveSpeed * 0.5f;
            baseWheelValue = Vector3.one;
        }

        public void OnMouseWheel(Vector2? input, bool wheel)
        {
            if (!MousePos()) return;
            isWheel = wheel;
            baseWheelValue = input;
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