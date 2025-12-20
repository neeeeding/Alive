using UnityEngine;

namespace _02Script.Inventory.Item
{
    public class ItemUse : MonoBehaviour
    {
        [SerializeField] private ItemCard cardData;

        private ItemType itemType;
        private bool isItem;

        private void Awake()
        {
            FindType();
        }

        public void Use()
        {
            if(itemType == ItemType.Nothing) FindType();

            if (!isItem)
            {
                if ((int)itemType % 1000 == 3) //부산물인 무기나 갑옷
                {
                    //Bus<ProduceEvent>.RaiseEvent(new  ProduceEvent(cardData.ReturnData().ReturnData()));
                }
            }
        }

        private void FindType()
        {
            itemType = cardData.ReturnData().ReturnData().itemType;
            isItem = cardData.ReturnData().ReturnData().isItem;
        }
    }
}