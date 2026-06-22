using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Battle.UI.Weapon;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Inventory
{
    public class ExplanationUI : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private GameObject side;
        [SerializeField] private BuffUI[] buff;
        [SerializeField] private BuffFind buffFind;
        
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemCount;
        [SerializeField] private TextMeshProUGUI itemExplanation;

        #region EnDi
        private void OnEnable()
        {
            InventoryItemCard.OnMouseCursor += Check;
            HideSide();
        }

        private void OnDisable()
        {
            InventoryItemCard.OnMouseCursor -= Check;
        }
        #endregion

        private void Check([CanBeNull] ItemDataSO item, int count,int star, float hp, WeaponArmorSaveData data = null)
        {
            if (item == null) {HideSide(); return; }
            
            SetExplanation(item);
            
            switch(item.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    itemCount.text = $"개수\n{count} / {item.maxCount}";
                    break;
                
                case ItemCategory.food:
                    itemCount.text = $"등급\n{star} 성";
                    break;
                
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    itemCount.text = $"내구도\n{hp} / 100";
                    break;
            }
            WeaponSideBuff(data);
        }

        //버프 & 디버프 관련 내용
        private void WeaponSideBuff(WeaponArmorSaveData data)
        {
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i].gameObject.SetActive(false);
            }
            
            if(data == null) return;
            
            List<BuffSO> buffs = new List<BuffSO>();
            
            foreach (BuffType buff in data.buffTypes)
            {
                buffs.Add(buffFind.GetBuff(buff));
            }
            if (buffs != null)
            {
                for (int i = 0; i < buffs.Count; i++)
                {
                    buff[i].gameObject.SetActive(true);
                    buff[i].BuffSet(buffFind.GetBuff(data.buffTypes[i]),null,EntityName.None,true);
                }
            }
            
        }

        private void SetExplanation(ItemDataSO item)
        {
            side.SetActive(true);
            itemImage.sprite = item.itemImage;
            itemName.text = item.itemName;
            itemExplanation.text = item.itemExplanation;
        }

        private void HideSide()
        {
            side.SetActive(false);
            itemImage.sprite = null;
            itemName.text = "";
            itemCount.text = "";
            itemExplanation.text = "";
        }
    }
}