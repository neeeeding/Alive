using _02Script.Inventory.Item;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Inventory
{
    public class InventorySelectUI : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private SerializedDictionary<ItemCategory, Transform> itemInventory;
        [SerializeField] private Button firstBtn;

        private ItemCategory category;
        
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void CategoryBtn(int i)
        {
            category = (ItemCategory)i;
            Select();
        }

        private void Select()
        {
            foreach (var item in itemInventory.Values)
            {
                item.gameObject.SetActive(false);
            }
            itemInventory[category].gameObject.SetActive(true);
            
            RectTransform rectT = itemInventory[category].GetComponent<RectTransform>();
            
            scroll.content = rectT;
            rectT.anchoredPosition = new  Vector2(rectT.anchoredPosition.x, 0); //맨 위
        }
    }
}