using System;
using _02Script.Item;
using UnityEngine;

namespace _02Script.UI.Store
{
    public class Store : MonoBehaviour
    {
        public static Action<ItemSO> OnSellItem; //아이템을 샀다

        [SerializeField] private GameObject sellCard; //판매 카드
        [Space(10f)] [SerializeField] private ItemSO[] allItemSOs; //아이템 모두

        [SerializeField] private int sellCount = 10; //하루 팔 아이템 양
        [SerializeField] private ItemSO[] sellItem; //팔 아이템들

        [SerializeField] private bool isCoin; //true : 코인 상점, false : 게임 상점

        private void Awake()
        {
            AddCard();
            DecideSellItem();
        }

        private void DecideSellItem() //팔 아이템 정하기
        {
            sellItem = new ItemSO[isCoin ? allItemSOs.Length : sellCount];

            if (!isCoin)
            {
                int maxNum = allItemSOs.Length - 1;
                int[] randomItmeNums = new int[sellCount];

                for (int i = 0; i < sellCount; i++)
                {
                    int random = UnityEngine.Random.Range(0, maxNum);
                    if (Array.Exists(randomItmeNums, x => x == random))
                    {
                        i--;
                        continue;
                    }
                    else
                    {
                        randomItmeNums[i] = random;
                        sellItem[i] = allItemSOs[random];
                    }
                }
            }

            ChildStore();
        }

        private void AddCard()
        {
            for (int i = 0; i < sellCount; i++)
            {
                GameObject card = Instantiate(sellCard, transform);
            }
        }

        private void ChildStore()
        {
            StoreSell[] stores = GetComponentsInChildren<StoreSell>();

            for (int i = 0; i < sellCount; i++)
            {
                stores[i].ShowSet(isCoin ? allItemSOs[i] : sellItem[i]);
            }
        }
    }
}