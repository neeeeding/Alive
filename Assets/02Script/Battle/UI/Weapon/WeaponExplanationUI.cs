using _02Script.Battle.UI.Etc;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponExplanationUI: ExplanationUI
    {
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

            UIShow(cardPos);
            _isEnter = true;
        }
    }
}