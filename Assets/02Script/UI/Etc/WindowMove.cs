using UnityEngine;

namespace _02Script.UI.Etc
{
    public class WindowMove : MonoBehaviour
    {
        [SerializeField] protected GameObject moveObj;
        
        private bool _isMove;
        private Vector3 _offset;

        protected virtual void OnEnable()
        {
            MouseCancel();
        }

        public virtual void MouseClick()
        {
            _isMove = true;
            _offset = moveObj.transform.position - Input.mousePosition;
        }

        public virtual void MouseCancel()
        {
            _isMove = false;
        }

        private void Update()
        {
            if(!_isMove) return;
            Move();
        }

        private void Move()
        {
            moveObj.transform.position = Input.mousePosition;
            moveObj.transform.position += _offset;
        }
    }
}