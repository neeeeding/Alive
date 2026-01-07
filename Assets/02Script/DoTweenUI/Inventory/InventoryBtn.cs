using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.DoTweenUI.Inventory
{
    public class InventoryBtn : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private bool everyClick = false;
        [SerializeField] private float downY;
        [SerializeField] private float delay = 1;
        
        private RectTransform rectTransform;
        private Vector3 targetPos;
        private Vector3 basePos;

        private Button btn;

        private void Awake()
        {
            Set();
        }

        private void OnEnable()
        {
            if(!everyClick) return;
            if (rectTransform == null) Set();
            
            btn.onClick.Invoke();
        }

        private void Set()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            basePos = rectTransform.position;
            targetPos = basePos + (Vector3.down * downY);
            
            rectTransform.DOMove(targetPos, delay);
            btn = GetComponent<Button>();
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