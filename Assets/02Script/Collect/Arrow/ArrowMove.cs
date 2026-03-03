using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Collect.Arrow
{
    public class ArrowMove : MonoBehaviour
    {
        [SerializeField] private Image arrow;
        [SerializeField] private Camera cam;

        [SerializeField]private GameObject _character;
        [SerializeField] private ArrowManager _arrowManager;
        private ArrowDirection _direction;

        private void Update()
        {
            print(cam.WorldToScreenPoint(_character.transform.position));
            if(!ShowCheck()) return;
            
            Move();
        }

        private bool ShowCheck() //화살표가 보여야 하는지
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

        private void Move()
        {
            Rect rect = _arrowManager.SetDirection(_direction, transform);
            float maxMin = 0;
            
            Vector2 objPos = cam.WorldToScreenPoint(_character.transform.position);
            Vector2 target = Vector2.zero;

            if (_direction == ArrowDirection.Up || _direction == ArrowDirection.Down)
            {
                maxMin = rect.x - (rect.width/2);
                target.x = Mathf.Clamp(objPos.x, maxMin, 1920);
                target.y = rect.y;
            }
            else
            {
                maxMin = rect.y - (rect.height/2);
                target.y = Mathf.Clamp(objPos.y,maxMin,1920);
                target.x = rect.x;
            }
            
            transform.position = target;
        }

        public void SetCharacter(GameObject obj, ArrowManager manager)
        {
            _character = obj;
            _arrowManager = manager;
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