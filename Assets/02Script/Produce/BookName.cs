using _02Script.Inventory.Item;
using _02Script.Produce.Weapon;
using TMPro;
using UnityEngine;

namespace _02Script.Produce
{
    public class BookName : MonoBehaviour
    {
        [SerializeField] private GameObject nameObject;
        [SerializeField] private TextMeshProUGUI nameText;

        private RectTransform rect;
        private void OnEnable()
        {
            ProduceBookCard.OnMouseCursor += ShowName;
            InventoryItemCard.OnMouseClick += Show;
            SelectItemCard.OnMouseCursor += ShowName;
            nameObject.SetActive(false);
            rect = nameObject.GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            ProduceBookCard.OnMouseCursor -= ShowName;
            InventoryItemCard.OnMouseClick -= Show;
            SelectItemCard.OnMouseCursor -= ShowName;
        }

        private void Show(ItemCard card, RectTransform parent, float selfCheck)
        {
            if(card == null){nameObject.SetActive(false); return;}
            ShowName(card.ReturnData().ReturnDataSO(), parent);
        }

        private void ShowName(ItemDataSO data, RectTransform parent)
        {
            nameObject.SetActive(data != null);

            if (data != null)
            {
                nameText.text = data.itemName;
                Vector3 pos = parent.position;
                pos.y += (parent.sizeDelta.y /2)+(rect.sizeDelta.y/2);
                if(pos.y >= 1080)
                    pos.y -= (parent.sizeDelta.y)+(rect.sizeDelta.y);
                rect.position = pos;
            }
        }
    }
}