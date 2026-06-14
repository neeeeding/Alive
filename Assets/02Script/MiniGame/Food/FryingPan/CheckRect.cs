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

        public RectTransform  ReturnRect()
        {
            return _transform;
        }

        public bool Check(RectTransform target) //겹치는가?
        {
            Vector3[] corners1 = new Vector3[4];
            Vector3[] corners2 = new Vector3[4];

            _transform.GetWorldCorners(corners1);
            target.GetWorldCorners(corners2);

            Rect r1 = new Rect(
                corners1[0].x,
                corners1[0].y,
                corners1[2].x - corners1[0].x,
                corners1[2].y - corners1[0].y
            );

            Rect r2 = new Rect(
                corners2[0].x,
                corners2[0].y,
                corners2[2].x - corners2[0].x,
                corners2[2].y - corners2[0].y
            );

            return r1.Overlaps(r2);
        }
    }
}