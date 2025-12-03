using _02Script.Item;
using _02Script.Manager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _02Script.UI.Store
{
    public class StoreSell : MonoBehaviour
    {
        [SerializeField] private ItemSO so; //아이템
        private Image itmeImgae; //생긴거
        private TextMeshProUGUI coinText; //제시 가격

        private void Awake()
        {
            coinText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ShowSet(ItemSO itemso)
        {
            so = itemso;
            //itmeImgae.sprite = so.itemImage;
            coinText.text = so.sellCoin.ToString();
        }

        public void ClickSell()
        {
            if (so.category == ItemCategory.coin)
            {
                GameManager.Instance.AddCoin(+so.sellCoin);

                Store.OnSellItem?.Invoke(so);
                GameManager.CoinText?.Invoke();
            }
            else
            {
                if (GameManager.Instance.PlayerStat.playerCoin >= so.sellCoin)
                {
                    GameManager.Instance.AddCoin(-so.sellCoin);

                    Store.OnSellItem?.Invoke(so);
                    GameManager.CoinText?.Invoke();
                    print("안목이 좋은걸? 그거 꽤 힘들게 얻었다고~");
                }
                else
                {
                    print("어이구 손님, 돈이 부족한 모양인데?");
                }
            }
        }
    }
}