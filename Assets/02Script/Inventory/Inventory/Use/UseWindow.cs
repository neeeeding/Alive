using _02Script.DoTweenUI.Warring;
using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Inventory.Use
{
    public class UseWindow  : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private TMP_InputField countInputField;
        [SerializeField] private Slider countSlider;

        private ItemCard card;
        private int maxNum;

        public void SetData(ItemCard data)
        {
            card = data;
            ItemData so = data.ReturnData();
            maxNum = so.ItemCount();
            countSlider.value = (float)1/maxNum;
            countInputField.text = 1.ToString();
        }
        
        public void SliderMove()
        {
            float x = countSlider.value;
            x *= maxNum;
            countInputField.text = ((int)x).ToString();
        }

        public void InputFieldInput()
        {
            int x = int.Parse(countInputField.text);
            countSlider.value = (float)x/maxNum;
        }

        public void HoldData()
        {
            inventoryManager.HoldItem(card.ReturnData(),int.Parse(countInputField.text));
            
            gameObject.SetActive(false);
        }

        public void UseData()
        {
            int rand = 1;
            if (card.ReturnData().ReturnDataSO().category == ItemCategory.food ||
                card.ReturnData().ReturnDataSO().category == ItemCategory.seed)
            {
                rand = Random.Range(0,6 -card.ReturnNum(true));
            }

            if (rand == 1)
            {
                WarringManager.Warring.ShowWarring("섭취에 성공하셨습니다!");
                inventoryManager.UseItem(card.ReturnData().ReturnDataSO(),int.Parse(countInputField.text));
            }
            else
            {
                WarringManager.Warring.ShowWarring("섭취에 실패하셨습니다...");
                inventoryManager.ThrowItem(card.ReturnData().ReturnDataSO(),int.Parse(countInputField.text));
            }
            
            gameObject.SetActive(false);
        }
        
        public void ThrowData()
        {
            inventoryManager.ThrowItem(card.ReturnData().ReturnDataSO(),int.Parse(countInputField.text));
            
            gameObject.SetActive(false);
        }
    }
}