using _02Script.Inventory.Item;
using TMPro;
using UnityEngine;

namespace _02Script.Inventory.Inventory.Use
{
    public class UseInventoryWindow : MonoBehaviour
    {
        //사용 창
        [SerializeField] private UseWindow useWindow;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI maxCount;

        public void Hide()
        {
            useWindow.gameObject.SetActive(false);
        }
        
        #region EnDi
        private void OnEnable()
        {
            InventoryItemCard.OnMouseClick += Show;
            Hide();
        }
        private void OnDisable()
        {
            InventoryItemCard.OnMouseClick -= Show;
        }
        #endregion

        private void Show(ItemCard card,RectTransform cardPos, float selfCheck/*본인인지 검사*/)
        {
            useWindow.gameObject.SetActive(true);
            
            useWindow.SetData(card,selfCheck);

            ItemDataSO data = card.ReturnData().ReturnDataSO();
            itemName.text = data.itemName;
            
            maxCount.text =  card.ReturnData().ItemCount().ToString();
            
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