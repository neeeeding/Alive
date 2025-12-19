using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace JYE._01Script.UI.Inventory
{
    public class InventoryBtn : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private float downY;
        [SerializeField] private float delay = 1;
        [Header("Need")]
        [SerializeField] private Image btnSprite;
        
        private RectTransform rectTransform;
        private Vector3 targetPos;
        private Vector3 basePos;

        private void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            basePos = rectTransform.position;
            targetPos = basePos + (Vector3.down * downY);
            
            rectTransform.DOMove(targetPos, delay);
        }

        public void MyBtnClick()
        {
            rectTransform.DOMove(basePos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
        }

        public void OtherBtnClick()
        {
            rectTransform.DOMove(targetPos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
        }
    }
}