using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Inventory
{
    public class InventorySelectUI : MonoBehaviour
    {
        /**처음 설정 용*/
        [Header("Setting")]
        [SerializeField]private bool isItem = true;
        [Header("Need")]
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private GameObject itemInventory;
        [SerializeField] private GameObject etcInventory;

        private void OnEnable()
        {
            Select(isItem);
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void ItemSelect()
        {
            Select(true);
        }

        public void EtcSelect()
        {
            Select(false);
        }

        /**true : 아이템 / false : 부산물*/
        private void Select(bool select)
        {
            itemInventory.SetActive(select);
            etcInventory.SetActive(!select);
            
            RectTransform rectT = select? itemInventory.GetComponent<RectTransform>() : 
                etcInventory.GetComponent<RectTransform>();
            
            scroll.content = rectT;
            rectT.anchoredPosition = new  Vector2(rectT.anchoredPosition.x, 0); //맨 위
        }
    }
}