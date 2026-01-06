using System;
using UnityEngine;
using DG.Tweening;

namespace _02Script.DoTweenUI.Popup
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private Vector2 ScaleVector;
        [SerializeField] private float delay = 1;
        
        private RectTransform rectTransform;
        private Vector3 targetScale;
        private Vector3 baseScale;
        
        [ContextMenu("Close")]
        public void Close()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOScale(baseScale, delay).SetEase(Ease.InOutQuad).SetUpdate(true));
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
            rectTransform.localScale = baseScale;
            targetScale = new Vector3(baseScale.x + ScaleVector.x, baseScale.y + ScaleVector.y, 0);
            
            rectTransform.DOScale(targetScale, delay).SetEase(Ease.OutCirc).SetUpdate(true);
        }

        private void Set()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            baseScale = rectTransform.localScale;
            baseScale = new Vector3(baseScale.x - ScaleVector.x, baseScale.y - ScaleVector.y, 0);
        }
    }
}