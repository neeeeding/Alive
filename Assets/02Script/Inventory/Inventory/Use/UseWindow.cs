using JYE._01Script.Inventory.Item;
using UnityEngine;

namespace JYE._01Script.Inventory.Inventory.Use
{
    public class UseWindow  : MonoBehaviour
    {
        [SerializeField] private ItemCard card;

        public void SetData(ItemCard data)
        {
            card = data;
        }

        public void UseData()
        {
            card.ReturnData().UseItem();
            card.UpdateCountUI();
            gameObject.SetActive(false);
        }
    }
}