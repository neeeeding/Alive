using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Buff
{
    public class BuffExplanationUI : MonoBehaviour
    {
        [SerializeField] private GameObject explanationUI;
        [Header("UI")]
        [SerializeField] private Image buffImage;
        [SerializeField] private TextMeshProUGUI buffNameText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI explanationText;
        [Header("Need")]
        [SerializeField] private Camera battleCamera;
        [SerializeField] private Transform battleCanvas;

        private readonly float _addY = 100;
        private readonly float _minX = 200;
        private readonly float _maxX = 1720;
        private readonly float _minY = 120;
        private readonly float _maxY = 370;
        

        private void OnEnable()
        {
            UIHide();
            Buff.OnMouseEnter += UIShow;
        }

        private void OnDisable()
        {
            Buff.OnMouseEnter -= UIShow;
        }

        private void UIShow(BuffSO so, float curSec, Vector3 cardPos, bool isUI)
        {
            if (so == null)
            {
                UIHide();
                return;
            }

            Time.timeScale = 0.2f;
            
            buffImage.sprite = so.buffImage;
            buffNameText.text = so.buffName;
            timeText.text = $"({(int)(so.buffDelay-curSec)}초 남음)";
            explanationText.text = so.buffExplanation;

            Vector3 targetPos = isUI ? cardPos : battleCamera.WorldToScreenPoint(cardPos);
            targetPos.x = Mathf.Clamp(targetPos.x, _minX, _maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, _minY, _maxY) + battleCanvas.position.y;
            
            explanationUI.transform.position = targetPos;
            explanationUI.SetActive(true);
            
            targetPos.y += _addY;
            
            explanationUI.transform.DOMove(targetPos, 0.2f).SetEase(Ease.OutCirc).SetUpdate(true);
        }

        private void UIHide()
        {
            Time.timeScale = 1f;
            explanationUI.SetActive(false);
        }
    }
}