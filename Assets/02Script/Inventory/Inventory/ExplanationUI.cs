using _02Script.Inventory.Item;
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

        private void Check([CanBeNull] ItemDataSO item, int count,int star, float hp)
        {
            if (item == null) {HideSide(); return; }
            
            SetExplanation(item);
            
            switch(item.category)
            {
                case ItemCategory.seed:
                case ItemCategory.fruit:
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