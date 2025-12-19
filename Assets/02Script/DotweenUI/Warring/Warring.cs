using System;
using JYE._01Script.Etc;
using TMPro;
using UnityEngine;

namespace JYE._01Script.UI.Warring
{
    public class Warring :  MonoBehaviour
    {
        [SerializeField] private GameObject warringObj;
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            warringObj.SetActive(false);
        }

        public async void ShowWarring(string massage = "오류가 발생했습니다.")
        {
            text.text = massage;
            warringObj.SetActive(true);
            await AsyncTime.WaitSeconds(1f);
            warringObj.SetActive(false);
        }
    }
}