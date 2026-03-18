using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce.Weapon
{
    public class SelectItemCardManager : MonoBehaviour
    {
        [SerializeField] private SelectItemCard card;
        [SerializeField] private StuffItemDataSO[] data;
        [SerializeField] private Transform drag;
        [SerializeField] private Transform parent;
        [SerializeField] private InventoryManager inventory;

        private void Start()
        {
            CardSet();
        }

        private void CardSet()
        {
            int index = 0;
            foreach (StuffItemDataSO item in data)
            {
                SelectItemCard newCard = Instantiate(card, parent);
                newCard.gameObject.SetActive(true);
                newCard.SetCard(index++,item, parent, drag, inventory);
            }
        }
    }
}