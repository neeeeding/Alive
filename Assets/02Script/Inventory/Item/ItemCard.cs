using _02Script.Produce;
using _02Script.Produce.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Item
{
    public class ItemCard :  MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected TextMeshProUGUI countUI;
        [SerializeField] protected Image cardImage;
        
        protected ItemData itemData;
        
        protected float star; //성 (1 ~ 5)
        protected float itemHp; //내구도 (0 ~ 100)
        protected WeaponArmorSaveData weaponArmorBuff;

        public ItemData ReturnData()
        {
            return itemData;
        }

        public float ReturnNum(bool isStar)
        {
            return isStar? star:itemHp;
        }
        
        public void ItemDamage(float damage)
        {
            itemHp -= damage;
            weaponArmorBuff.hp -= damage;
        }

        public virtual void NewCard(ItemData itemData,int setStar = 0, int setItemHp = 100,WeaponArmorSaveData saveData = null)
        {
            this.itemData = itemData;
            cardImage.sprite = this.itemData.ReturnDataSO().itemImage;
            weaponArmorBuff = saveData;
            star = setStar;
            itemHp = setItemHp;
        }

        protected virtual void OnEnable()
        {
            UpdateCountUI();
        }

        /**개수*/
        public virtual void UpdateCountUI()
        {
            if(itemData == null) return;
            
            int count = itemData.ItemCount();
            
            countUI.text = count.ToString();

            ItemCategory category = itemData.ReturnDataSO().category;

            if (category != ItemCategory.seed && category != ItemCategory.special &&
                category != ItemCategory.viand && category != ItemCategory.stuff)
            {
                countUI.text = "";
            }
            
            if (count <= 0 && !(itemData.ReturnDataSO() is ProduceBookSO))
            {
                gameObject.SetActive(false);
            }
        }
    }
}