using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Inventory
{
    public class InventoryItemShow : MonoBehaviour
    {
        [SerializeField] private Image synergyImage;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI explanation;
        
        #region EnDi
        private void OnEnable()
        {
            InventoryItemCard.OnMouseCursor += Show;
            Show(null,0);
        }
        private void OnDisable()
        {
            InventoryItemCard.OnMouseCursor -= Show;
        }
        #endregion

        private void Show(ItemDataSO so, int count)
        {
            synergyImage.sprite = so.itemImage;

            countText.text = $"{count}/{so.maxCount}";
            
            nameText.text = so.itemName;
            explanation.text = so.itemExplanation;
        }
    }
}