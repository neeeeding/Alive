using _02Script.Battle.UI.Etc;
using UnityEngine;

namespace _02Script.Battle.Buff
{
    public class BuffExplanationUI : ExplanationUI
    {
        [SerializeField] private float _baseY = 20;
        [SerializeField] protected Camera cam;

        protected override void OnEnable()
        {
            base.OnEnable();
            BuffUI.OnMouseEnter += UIShow;
        }

        private void OnDisable()
        {
            BuffUI.OnMouseEnter -= UIShow;
        }

        private void UIShow(BuffSO so, float curSec, Vector3 cardPos, bool isUI)
        {
            if (so == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }
            
            image.sprite = so.buffImage;
            nameText.text = so.buffName;
            explanationText.text = so.buffExplanation;
            etcText.text = $"({(int)(so.buffDelay-curSec)}초 남음)";
            if (curSec < 0)
            {
                etcText.text = "";
            }

            cardPos.y += _baseY;
            UIShow(isUI ? cardPos : cam.WorldToScreenPoint(cardPos));
        }
    }
}