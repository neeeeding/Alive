using _02Script.Manager;
using UnityEngine;
using TMPro;

namespace _02Script.UI.InGame
{
    public class CoinCount : MonoBehaviour
    {
        private TextMeshProUGUI coinText;

        private void Awake()
        {
            coinText = GetComponentInChildren<TextMeshProUGUI>();
            Text();
            GameManager.OnStart += GameStart;
        }

        private void GameStart()
        {
            GameManager.CoinText += Text;
        }

        private void Text()
        {
            coinText.text = GameManager.Instance.PlayerStat.playerCoin.ToString();
        }

        private void OnDisable()
        {
            GameManager.OnStart -= GameStart;
            GameManager.CoinText -= Text;
        }
    }
}
