using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Etc
{
    public class ExplanationUI: MonoBehaviour
    {
        [SerializeField] protected RectTransform explanationUI;
        [Header("UI")]
        [SerializeField] protected Image image;
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI explanationText;
        [SerializeField] protected TextMeshProUGUI etcText;
        [Header("Need")]
        [SerializeField] protected Transform canvas;

        [SerializeField]protected float addY = 100;
        protected float minX = 0;
        protected float maxX = 1920;
        protected float minY = 0;
        protected float maxY = 540;
        
        protected bool _isEnter;

        
        #region Mouse
        public void MouseEnter()
        {
            _isEnter = true;
        }        
        public void MouseExit()
        {
            _isEnter = false;
            UIHide();
        }

        protected bool IsMouseOver()
        {
            return RectTransformUtility.RectangleContainsScreenPoint(explanationUI, Input.mousePosition, null);
        }
        #endregion

        private void Update()
        {
            if(!_isEnter) return;

            _isEnter = IsMouseOver();
        }
        
        protected virtual void OnEnable()
        {
            UIHide();
            SetMinMax();
        }

        protected virtual void UIShow(Vector3 cardPos)
        {
            Time.timeScale = 0.2f;

            Vector3 targetPos = cardPos;
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY) + canvas.position.y;
            
            explanationUI.position = targetPos;
            explanationUI.gameObject.SetActive(true);
            
            targetPos.y += addY;
            if (targetPos.y > maxY)
            {
                targetPos.y -= addY*2;
            }
            
            explanationUI.DOMove(targetPos, 0.2f).SetEase(Ease.OutCirc).SetUpdate(true);
        }

        protected virtual void UIHide()
        {
            Time.timeScale = 1f;
            explanationUI.gameObject.SetActive(false);
        }

        protected void SetMinMax()
        {
            Rect rect = explanationUI.rect;
            
            minX = rect.width/2;
            maxX -= minX;
            minY = rect.height;
            maxY -= minY;
        }
    }
}