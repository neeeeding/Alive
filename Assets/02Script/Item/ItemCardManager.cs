using System.Collections.Generic;
using _02Script.Manager;
using _02Script.UI.Save;
using UnityEngine;

namespace _02Script.Item
{
    public class ItemCardManager : MonoBehaviour
    {
        [SerializeField] private ItemSO[] itemSOs; //들기가 가능한 아이템
        [SerializeField] private GameObject itemCard; //아이템 카드
        [SerializeField] private ItemHold item; //아이템

        private static List<ItemSO> items = new List<ItemSO>(); //비활성화 하는 애 때문에
        private bool isSetting; //세팅 여부

        private void Awake()
        {
            isSetting = false;
            GameManager.OnStart += GameStart;
        }

        private void OnEnable()
        {
            LoadCard.OnLoad += LoadItem;
            
            if(GameManager.Instance.isStart)
            {
                if (!isSetting)
                {
                    AddAllItems();
                }
                LoadItem();
            }
        }

        private void GameStart() //스타트
        {
            AddAllItems();
        }

        private void AddAllItems() //모든 아이템들 로드 (추가)
        {
            item = GameManager.Instance.itemPos;

            isSetting = true;
            for(int i = 0; i < itemSOs.Length; i++)
            {
                items.Add(itemSOs[i]);
                GameObject card = Instantiate(itemCard, transform);
                ItemCard cardSc = card.GetComponent<ItemCard>();
                cardSc.SetCard(items[i], item);
            }
        }

        private void LoadItem() //로드
        {
            for(int i = 0; i< itemSOs.Length; i++)
            {
                ItemSO so = items[i];
                ActionItemActive(so,true);

            }
        }

        private void GetItem(ItemSO so) //얻은 아이템
        {
            ActionItemActive(so, false);
        }

        private void ActionItemActive(ItemSO so, bool b)
        {

            foreach (Transform card in gameObject.transform)
            {
                if (card.TryGetComponent(out ItemCard cardSc))
                {
                    card.gameObject.SetActive(cardSc.HaveItem(so,b)); //얻은 아이템에 따라 카드 활성화
                }
            }
        }

        private void OnDisable()
        {
            GameManager.OnStart -= GameStart;
            LoadCard.OnLoad -= LoadItem;
        }
    }
}
