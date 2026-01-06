using DG.Tweening;
using UnityEngine;

namespace _02Script.DoTweenUI.MoveUI
{
    public class MoveUI : MonoBehaviour
    {
        [SerializeField] private Vector2 MoveVector;
        [SerializeField] private float delay = 1;
        
        private RectTransform rectTransform;
        private Vector3 targetPos;
        private Vector3 basePos;
        
        [ContextMenu("Close")]
        public void Close()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOMove(basePos, delay).SetEase(Ease.InOutQuad).SetUpdate(true));
            seq.AppendCallback(() => gameObject.SetActive(false));
        }
        
        private void Awake()
        {
            Set();
        }
        
        private void OnEnable()
        {
            if (rectTransform == null)
            {
                Set();
            }
            rectTransform.position = basePos;
            targetPos = new Vector3(basePos.x + MoveVector.x, basePos.y + MoveVector.y, 0);
            
            rectTransform.DOMove(targetPos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
        }

        private void Set()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            basePos = rectTransform.position;
            basePos = new Vector3(basePos.x - MoveVector.x, basePos.y - MoveVector.y, 0);
        }
    }
}