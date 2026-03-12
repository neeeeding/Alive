using _02Script.Battle.Buff;
using _02Script.Battle.UI.Explanation;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponExplanationUI: ExplanationUI
    {
        [SerializeField] private BuffUI buff;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            WeaponInventoryCard.OnMouseEnter += UIShow;
            MouseExit();
        }

        private void OnDisable()
        {
            WeaponInventoryCard.OnMouseEnter -= UIShow;
        }

        private void UIShow(WeaponItemDataSO so, Vector3 cardPos)
        {
            if (so == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }
            
            image.sprite = so.itemImage;
            nameText.text = so.itemName;
            explanationText.text = so.itemExplanation;
            if (so.skillBuff != null)
            {
                buff.gameObject.SetActive(true);
                buff.BuffSet(so.skillBuff,null,EntityName.None,true);
            }
            else
            {
                buff.gameObject.SetActive(false);
            }

            UIShow(cardPos);
            _isEnter = true;
        }
    }
}