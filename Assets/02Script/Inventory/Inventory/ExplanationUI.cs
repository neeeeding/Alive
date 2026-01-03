using _02Script.Inventory.Item;
using _02Script.Item;
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

        private void Check([CanBeNull] ItemDataSO item, int count)
        {
            if (item == null) {HideSide(); return; }
            
            SetExplanation(item);
            itemCount.text = $"{count} / {item.maxCount}";
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