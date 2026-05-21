using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Collect.Arrow
{
    public class ArrowMove : MonoBehaviour
    {
        [SerializeField]protected ArrowManager _arrowManager;
        [SerializeField] protected Image arrow;
        [SerializeField] protected Camera cam;

        public GameObject _character;
        protected ArrowDirection _direction;

        private void Update()
        {
            if(!_character) return;
            if(!ShowCheck()) return;
            
            Move();
            Rotate();
        }

        protected virtual bool ShowCheck() //화살표가 보여야 하는지
        {
            Vector2 camPos = cam.WorldToViewportPoint(_character.transform.position);

            if (Mathf.Abs(camPos.x) > Mathf.Abs(camPos.y))
            {
                _direction = camPos.x > 1 ? ArrowDirection.Right :
                    camPos.x < 0 ? ArrowDirection.Left : ArrowDirection.None;
            }
            else
            {
                _direction = camPos.y > 1 ? ArrowDirection.Up :
                    camPos.y < 0 ? ArrowDirection.Down : ArrowDirection.None;
            }
            
            arrow.gameObject.SetActive(_direction != ArrowDirection.None);
            
            return _direction != ArrowDirection.None;
        }

        protected virtual void Move()
        {
            Rect rect = _arrowManager.SetDirection(_direction, transform);
            
            Vector2 objPos = cam.WorldToScreenPoint(_character.transform.position);
            Vector2 target = Vector2.zero;

            if (_direction == ArrowDirection.Up || _direction == ArrowDirection.Down)
            {
                target.x = Mathf.Clamp(objPos.x, rect.x - (rect.width/2), rect.x + (rect.width/2));
                target.y = rect.y;
            }
            else
            {
                target.y = Mathf.Clamp(objPos.y,rect.y - (rect.height/2),rect.y + (rect.height/2));
                target.x = rect.x;
            }
            
            transform.position = target;
        }

        private void Rotate()
        {
            float angle = Mathf.Atan2(_character.transform.position.y - cam.transform.position.y, 
                               _character.transform.position.x - cam.transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        public void SetCharacter(GameObject obj,Color color ,ArrowManager manager)
        {
            _character = obj;
            _arrowManager = manager;
            arrow.color = color;
        }
    }

    public enum ArrowDirection
    {
        None = 0,
        Up,
        Down,
        Left,
        Right,
    }
}