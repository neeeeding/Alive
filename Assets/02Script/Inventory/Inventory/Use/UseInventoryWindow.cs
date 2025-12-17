using JYE._01Script.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JYE._01Script.Inventory.Inventory.Use
{
    public class UseInventoryWindow : MonoBehaviour
    {
        //사용 창
        [SerializeField] private UseWindow useWindow;
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemName;

        public void Hide()
        {
            useWindow.gameObject.SetActive(false);
        }
        
        #region EnDi
        private void OnEnable()
        {
            InventoryItemCard.OnMouseClick += Show;
            useWindow.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            InventoryItemCard.OnMouseClick -= Show;
        }
        #endregion

        private void Show(ItemCard card,RectTransform cardPos)
        {
            useWindow.gameObject.SetActive(false);
            useWindow.gameObject.SetActive(true);
            
            useWindow.SetData(card);

            ItemData data = card.ReturnData();
            
            itemImage.sprite = data.ReturnData().itemImage;
            itemName.text = data.ReturnData().itemName;
            
            WindowPos(cardPos);
        }

        private void WindowPos(RectTransform cardPos)
        {
            float addY = cardPos.position.y + cardPos.sizeDelta.y;
            //음 양
            useWindow.gameObject.GetComponent<RectTransform>().position = cardPos.position +
                                                                          (Vector3.up * cardPos.sizeDelta.y * (addY >= 1000 ? -1 : 1));
            
        }
    }
}