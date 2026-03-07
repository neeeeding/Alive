using _02Script.Battle.Buff;
using _02Script.Battle.UI.Etc;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponExplanationUI: ExplanationUI
    {
        [SerializeField] private BuffUI _buff;
        
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
                _buff.gameObject.SetActive(true);
                _buff.BuffSet(so.skillBuff,null,EntityName.None,true);
            }
            else
            {
                _buff.gameObject.SetActive(false);
            }

            UIShow(cardPos);
            _isEnter = true;
        }
    }
}