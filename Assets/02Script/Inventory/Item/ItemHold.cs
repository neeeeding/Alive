using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    public class ItemHold : MonoBehaviour
    {
        private ItemData holdData;
        private int holdCount;
    
        public void Setting(ItemData currentData,  int count = 1)
        {
            holdData = currentData;
            holdCount = count;
        }

        public void UseItem()
        {
            
        }
    }
}
