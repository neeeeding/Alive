using System;
using TMPro;
using UnityEngine;

namespace _02Script.UI.Chat
{
    public class SelectBtn : MonoBehaviour
    {
        public static Action<int> OnSelect; //선택한 번호

        private int mySelectNum; //본인의 번호 (챕터에서, 넘버)
        [SerializeField] private TextMeshProUGUI selectTexts; //선택지 대화

        public void SetSelect(string text, int thisNum) //대화 내용, 선택지 번호
        {
            selectTexts.text = text;
            mySelectNum = thisNum;
        }

        public void ClcikSelect() //선택 버튼 누를 때
        {
            OnSelect?.Invoke(mySelectNum + 1 /*0부터 시작하니*/);
        }
    }
}