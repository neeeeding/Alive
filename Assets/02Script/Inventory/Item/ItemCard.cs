using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JYE._01Script.Inventory.Item
{
    public class ItemCard :  MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected TextMeshProUGUI countUI;
        [SerializeField] protected Image cardImage;
        
        protected ItemData itemData;

        public ItemData ReturnData()
        {
            return itemData;
        }

        public virtual void NewCard(ItemData itemData)
        {
            this.itemData = itemData;
            cardImage.sprite = this.itemData.ReturnData().itemImage;
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

            if (itemData.ReturnData().itemType == ItemType.Book)
            {
                countUI.gameObject.SetActive(false);
            }
            else if (count <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}