using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundSelectStuffCardManager : MonoBehaviour
    {
        [SerializeField] private CompoundSelectStuffCard cardPrefabs;
        [SerializeField] private StuffItemDataSO[] data;
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
                CompoundSelectStuffCard newCard = Instantiate(cardPrefabs, parent);
                newCard.gameObject.transform.SetParent(parent);
                newCard.SetCard(item, inventory);
                newCard.gameObject.SetActive(true);
            }
        }
    }
}