using _02Script.Collect.Arrow;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Tutorial.House
{
    public class Guide : ArrowMove
    {
        public void SetTarget(GameObject target)
        {
            _character = target;
        }
        protected override void Update()
        {
            if(!ShowCheck()) return;
            
            Move();
            Rotate();
        }

        protected override bool ShowCheck()
        {
            arrow.gameObject.SetActive(_character != null);
            return _character != null;
        }

        protected override void Move()
        {
            base.Move();
            if(_direction != ArrowDirection.None) return;
            
            Rect rect = _arrowManager.SetDirection(_direction, transform);
            
            Vector2 objPos = cam.WorldToScreenPoint(_character.transform.position);
            Vector2 target = Vector2.zero;

            target.x = Mathf.Clamp(objPos.x, rect.x - (rect.width/2), rect.x + (rect.width/2));
            target.y = Mathf.Clamp(objPos.y,rect.y - (rect.height/2),rect.y + (rect.height/2));
            
            transform.position = target;
        }
    }
}