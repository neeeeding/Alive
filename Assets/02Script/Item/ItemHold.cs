using System.Collections.Generic;
using UnityEngine;

namespace _02Script.Item
{
    public class ItemHold : MonoBehaviour
    {
        private ItemDataSO dataSo;
        private ItemCard card;
    
        public void Setting(ItemDataSO currentDataSo, ItemCard currentCard)
        {
            dataSo = currentDataSo;
            card = currentCard;
        }

        private void UseItem()
        {
            card.HideItem();
        }
    }
}
