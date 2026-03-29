using _02Script.Battle.Buff;
using _02Script.Battle.UI.Explanation;
using _02Script.Battle.UI.Weapon;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
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
            MouseExit();
        }

        private void OnDisable()
        {
            CompoundSelectWeaponArmorCard.OnMouseEnter -= UIShow;
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
            
            explanationText.text = data.buffExplanation+ data.explanation;
            
            
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i].gameObject.SetActive(false);
            }
            
            if (data.buffTypes != null)
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