using UnityEngine;

namespace _02Script.MiniGame.Food.FryingPan
{
    public class CheckRect : MonoBehaviour
    {
        private RectTransform _transform;
        private Rect _checkRect;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _checkRect = _transform.rect;
        }

        public (Vector3, Rect) ReturnRect()
        {
            return (_transform.position, _checkRect);
        }

        public bool Check((Vector3, Rect) check)
        {
            return Check(check.Item1, check.Item2);
        }

        public bool Check(Vector3 pos, Rect rect) //겹치는가?
        {
            Rect r1 = new Rect(pos.x, pos.y, rect.width / 2, rect.height / 2);
            pos = _transform.position;
            Rect r2 = new Rect(pos.x, pos.y, rect.width / 2, rect.height / 2);
            
            return r1.Overlaps(r2);
        }
    }
}