using UnityEngine;

namespace _02Script.Item
{
    public class ItemHold : MonoBehaviour
    {
        private ItemSO so;
        private ItemCard card;
    
        public void Setting(ItemSO currentSO, ItemCard currentCard)
        {
            so = currentSO;
            card = currentCard;
        }

        private void UseItem()
        {
            card.HideItem();
        }
    }
}
