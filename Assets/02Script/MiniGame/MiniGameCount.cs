using System;
using _02Script.Etc;
using TMPro;
using UnityEngine;

namespace _02Script.MiniGame
{
    public class MiniGameCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;

        private void OnEnable()
        {
            Count();
        }

        private async void Count()
        {
            Time.timeScale = 0;
            countText.gameObject.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                countText.text = i.ToString();
                await AsyncTime.WaitSeconds(1, true);
            }
            Time.timeScale = 1;
            countText.gameObject.SetActive(false);
        }
    }
}