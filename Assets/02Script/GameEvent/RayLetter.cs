using TMPro;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class RayLetter : MonoBehaviour
    {
        [SerializeField] private string[] text;
        [SerializeField] private GameObject letter;
        [SerializeField] private TextMeshProUGUI textUI;

        private void Awake()
        {
            HideLetter();
        }

        public void ShowLetter(int page)
        {
            letter.SetActive(true);
            textUI.text = text[page];
        }

        public void HideLetter()
        {
            letter.SetActive(false);
        }
    }
}