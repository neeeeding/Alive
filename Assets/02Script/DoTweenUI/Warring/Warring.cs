using _02Script.Etc;
using TMPro;
using UnityEngine;

namespace _02Script.DoTweenUI.Warring
{
    public class Warring :  MonoBehaviour
    {
        [SerializeField] protected GameObject warringObj;
        [SerializeField] protected TextMeshProUGUI text;

        protected virtual void Awake()
        {
            warringObj.SetActive(false);
        }

        public virtual async void ShowWarring(string massage = "오류가 발생했습니다.", float i =1)
        {
            text.text = massage;
            warringObj.SetActive(true);
            await AsyncTime.WaitSeconds(i, true);
            warringObj.SetActive(false);
        }
    }
}