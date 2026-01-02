using _02Script.Item;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item/ItemDataSO")]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite itemImage;
        public int maxCount;
        
        public ItemCategory category; //카테고리
        public ItemType itemType;
        
        public string itemName;
        public string itemExplanation;

        public virtual bool DoSomething() //보통은 그냥 사용 못하게
        {
            return false;
        }
    }
}