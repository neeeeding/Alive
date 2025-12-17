using System;
using JYE._01Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JYE._01Script.Inventory.Inventory.Characteristic
{
    public class InventoryCharacteristicShow : MonoBehaviour
    {
        [SerializeField] private GameObject synergyUI;
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
            if (so == null || so.haveCharacteristic == null)
            {
                synergyUI.SetActive(false);
                return;
            }
            
            // characteristicImage.color = new  Color(1, 1, 1, 
            //     (so.mustItemCount == 0 || so.mustItemCount > count)? //미달?
            //         50/225f : 1);
            
            synergyUI.SetActive(true);
            synergyImage.sprite = so.haveCharacteristic.itemImage;

            countText.text = $"{count}/{so.haveCharacteristic.mustItemCount}";
            
            nameText.text = so.haveCharacteristic.itemName;
            explanation.text = so.haveCharacteristic.itemExplanation;

            if (so.mustItemCount == 0 || so.haveCharacteristic.mustItemCount > count)
            {
                synergyImage.color = new Color(1, 1, 1, 50/255f);
                explanation.text += $"\n(비활성화)";
            }
            else
            {
                synergyImage.color = new Color(1, 1, 1, 1);
                explanation.text += $"\n(활성화)";
            }
        }
    }
}