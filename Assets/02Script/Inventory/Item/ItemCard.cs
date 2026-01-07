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
        
        protected int star; //성 (1 ~ 5)
        protected int itemHp; //내구도 (0 ~ 100)

        public ItemData ReturnData()
        {
            return itemData;
        }

        public int ReturnNum(bool isStar)
        {
            return isStar? star:itemHp;
        }
        
        public void ItemDamage(int damage)
        {
            itemHp -= damage;
        }

        public virtual void NewCard(ItemData itemData, int setStar = 0, int setItemHp = 100)
        {
            this.itemData = itemData;
            cardImage.sprite = this.itemData.ReturnDataSO().itemImage;
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
                category != ItemCategory.fruit && category != ItemCategory.stuff)
            {
                countUI.text = "";
            }
            
            if (count <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}