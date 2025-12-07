using TMPro;
using UnityEngine;

namespace _02Script.UI.Dialog.Dialog
{
    public class DialogTextController :  MonoBehaviour
    {
        public void OneOne(string chatText, int nCount, TextMeshProUGUI dialogText,
            ref bool isTime) //한 글자 씩 출력
        {
            string[] chat = chatText.Split(' ');
            int j = 0;
    
            if (dialogText.text == chatText) {
                isTime = false;
                return;
            } else if (nCount == 1) {
                dialogText.text = " ";
            }

            foreach (string s in chat)
            {
                j++;
                if(j != nCount) continue;
                dialogText.text += s + " ";
            }

            if (nCount >= j)
            {
                isTime = false;
                dialogText.text = chatText;
            }
        }

        
        public string IsExchangeText(string dial, string text, string change) //어떤 문자를 다른 문자로 바꾸기 ( ` => , )
        {
            string changeDialog = "";

            if(dial.Contains(text)) //바꿀것이 포함 되어있는지
            {
                string[] saveText = dial.Split(text); //일단 쪼개주기
                for(int i = 0; i < saveText.Length -1; i++) //마지막은 미포함이니 빼주고 반복
                {
                    changeDialog += saveText[i];
                    changeDialog += change;
                }
                changeDialog += saveText[saveText.Length - 1]; //빼줬던 마지막 추가 해주기
            }
            else //없으면 그대로
            {
                changeDialog = dial;
            }

            return changeDialog;
        }
    }
}