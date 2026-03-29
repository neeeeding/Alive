using _02Script.Battle.Buff;
using _02Script.Battle.UI.Explanation;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Produce.Weapon.Compound.Explanation
{
    public class StuffExplanationUI: ExplanationUI
    {
        [SerializeField] private BuffUI[] buff;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CompoundSelectStuffCard.OnMouseEnter += UIShow;
            MouseExit();
        }

        private void OnDisable()
        {
            CompoundSelectStuffCard.OnMouseEnter -= UIShow;
        }

        private void UIShow(CompoundSelectStuffCard card, Vector3 cardPos)
        {
            if (card == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }

            StuffItemDataSO so = card.ItemData;
            
            image.sprite = so.itemImage;
            nameText.text = so.itemName;
            explanationText.text = so.itemExplanation;
            
            if (so.buffs != null)
            {
                for (int i = 0; i < so.buffs.Length; i++)
                {
                    buff[i].gameObject.SetActive(true);
                    buff[i].BuffSet(so.buffs[i],null,EntityName.None,true);
                }
            }
            else
            {
                for (int i = 0; i < buff.Length; i++)
                {
                    buff[i].gameObject.SetActive(false);
                }
            }

            UIShow(cardPos);
            _isEnter = true;
        }
        protected override void UIShow(Vector3 cardPos)
        {
            Time.timeScale = 0.2f;

            Vector3 targetPos = cardPos;
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX) + addY;
            
            explanationUI.position = targetPos;
            explanationUI.gameObject.SetActive(true);
        }
    }
}