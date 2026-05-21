using UnityEngine;

namespace _02Script.Collect.Arrow
{
    public class ArrowManager : MonoBehaviour
    {
        [SerializeField] private RectTransform up;
        [SerializeField] private RectTransform down;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform right;
        [SerializeField] private RectTransform none;

        public Rect SetDirection(ArrowDirection direction, Transform arrow)
        {
            RectTransform pos = direction switch
            {
                ArrowDirection.Up => up, 
                ArrowDirection.Down  => down,
                ArrowDirection.Left => left,
                ArrowDirection.Right  => right,
                ArrowDirection.None => none,
            };

            Rect rect = pos.rect;
            
            rect.x = pos.position.x;
            rect.y = pos.position.y;
            return rect;
        }
    }
}