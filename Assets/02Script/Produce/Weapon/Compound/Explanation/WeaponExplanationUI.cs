using _02Script.Battle.Buff;
using _02Script.Battle.UI.Explanation;
using _02Script.Battle.UI.Weapon;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using UnityEngine;

namespace _02Script.Produce.Weapon.Compound.Explanation
{
    public class WeaponExplanationUI: ExplanationUI
    {
        [SerializeField] private BuffUI[] buff;
        [SerializeField] private BuffFind buffFind;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            CompoundSelectWeaponArmorCard.OnMouseEnter += UIShow;
            PersonWeaponCard.OnMouseEnter += UIShow;
            MouseExit();
        }

        private void OnDisable()
        {
            CompoundSelectWeaponArmorCard.OnMouseEnter -= UIShow;
            PersonWeaponCard.OnMouseEnter -= UIShow;
        }

        private void UIShow(PersonWeaponCard card, Vector3 cardPos)
        {
            if (card == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }

            ItemDataSO so = card.ReturnData().ReturnDataSO();
            image.sprite = so.itemImage;
            nameText.text = so.itemName;
            explanationText.text = so.itemExplanation;
            
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i].gameObject.SetActive(false);
            }

            UIShow(cardPos);
            _isEnter = true;
        }

        private void UIShow(CompoundSelectWeaponArmorCard card, WeaponArmorSaveData data, Vector3 cardPos)
        {
            if (card == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }
            if(card.ReturnData().ReturnDataSO() is WeaponItemDataSO weapon )
            {
                image.sprite = weapon.itemImage;
                nameText.text = weapon.itemName;
            }
            if(card.ReturnData().ReturnDataSO() is ArmorItemDataSO Armorr )
            {
                image.sprite = Armorr.itemImage;
                nameText.text = Armorr.itemName;
            }

            if (data != null)
                explanationText.text = data.buffExplanation + data.explanation;
            else
                explanationText.text = card.ReturnData().ReturnDataSO().itemExplanation;
                    
            
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i].gameObject.SetActive(false);
            }
            
            if (data != null && data.buffTypes != null)
            {
                for (int i = 0; i < data.buffTypes.Count; i++)
                {
                    buff[i].gameObject.SetActive(true);
                    buff[i].BuffSet(buffFind.GetBuff(data.buffTypes[i]),null,EntityName.None,true);
                }
            }

            // cardPos.y -= addY;
            // cardPos.x += addY;

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