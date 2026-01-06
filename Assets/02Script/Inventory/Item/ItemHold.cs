using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Item
{
    public class ItemHold : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject holdUI;
        [SerializeField] private Image sprite;
        [SerializeField] private TextMeshProUGUI nameT;
        [SerializeField] private TextMeshProUGUI countT;
        private ItemData holdData;
        private int holdCount;

        [Header("In Game")]
        [SerializeField] private GameObject holdItem;
        private SpriteRenderer myRend;
        
        private void Awake()
        {
            myRend = holdItem.GetComponent<SpriteRenderer>();
            holdItem.SetActive(false);
            holdUI.SetActive(false);
        }

        public void Setting(ItemData currentData,  int count = 1)
        {
            holdData = currentData;
            holdCount = count;
            
            holdUI.SetActive(true);
            sprite.sprite = holdData.ReturnDataSO().itemImage;
            nameT.text = holdData.ReturnDataSO().itemName;
            countT.text = holdCount.ToString();
            
            myRend.sprite = holdData.ReturnDataSO().itemImage;
            holdItem.SetActive(true);
        }

        public void CheckLessItem()
        {
            if(holdData == null) {holdItem.SetActive(false); holdUI.SetActive(false); return;}
            
            if (holdData.ItemCount() < holdCount)
            {
                holdCount =  holdData.ItemCount();
            }

            if (holdCount <= 0)
            {
                holdCount = 0;
                UseItem();
            }
        }
        
        [ContextMenu("UseItem")]
        public void UseItem()
        {
            if(holdData == null) {holdItem.SetActive(false); holdUI.SetActive(false); return;}
            
            holdData.UseItem(holdCount,true);
            
            holdData = null;
            holdCount = 0;
            holdItem.SetActive(false);
            holdUI.SetActive(false);
        }
    }
}
