using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Battle.UI.Explanation;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponExplanationUI: ExplanationUI
    {
        [SerializeField] private BuffUI[] buff;
        
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

        private void UIShow(WeaponItemDataSO so,WeaponArmorSaveData data, List<BuffSO> buffs, Vector3 cardPos)
        {
            if (so == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }
            
            image.sprite = so.itemImage;
            nameText.text = so.itemName;
            explanationText.text = data.explanation;
            
            if (buffs != null)
            {
                for (int i = 0; i < buffs.Count; i++)
                {
                    buff[i].gameObject.SetActive(true);
                    buff[i].BuffSet(so.skillBuff,null,EntityName.None,true);
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
    }
}