using _02Script.Inventory.Item;
using _02Script.Item;
using UnityEngine;
using UnityEngine.UI;
using ItemCard = _02Script.Inventory.Item.ItemCard;

namespace _02Script.Inventory.Inventory.Use
{
    public class UseWindow  : MonoBehaviour
    {
        [SerializeField] private ItemCard card;
        [SerializeField] private InputField countInputField;
        
        [SerializeField] private Slider countSlider;

        private int maxNum;

        public void SetData(ItemCard data)
        {
            card = data;
            ItemData so = data.ReturnData();
            maxNum = so.ItemCount();
            countSlider.value = (float)1/maxNum;
            countInputField.text = 1.ToString();
        }
        
        public void SliderMove(int x)
        {
            x *= maxNum;
            countInputField.text = x.ToString();
        }

        public void InputFieldInput()
        {
            int x = int.Parse(countInputField.text);
            countSlider.value = (float)x/maxNum;
        }

        public void UseData()
        {
            card.ReturnData().UseItem(int.Parse(countInputField.text));
            card.UpdateCountUI();
            gameObject.SetActive(false);
        }
        
        public void ThrowData()
        {
            card.ReturnData().ThrowItem(int.Parse(countInputField.text));
            card.UpdateCountUI();
            gameObject.SetActive(false);
        }
    }
}