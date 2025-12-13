using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using TMPro;
using UnityEngine;

namespace _02Script.UI.Dialog.SpeechBubble
{
    public class ShowSpeechBubble : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private GameObject speechBubble; //말풍선
        [SerializeField] private TextMeshProUGUI bubbleText; //텍스트
        //스크립트
        [SerializeField] private DialogTextController  dialogTextController; //텍스트 출력 관련
        
        private string word; //내용
        private int index; //단어 수
        private bool isChat; //출력 하는 중인지
        private float curTime;
        
        private void OnEnable()
        {
            word =  "......";
            speechBubble.SetActive(false);
            DialogEntity.OnCanDialog += Show;
        }

        private void OnDisable()
        {
            DialogEntity.OnCanDialog -= Show;
        }

        private void Update()
        {
            if (isChat)
            {
                if (curTime > 0.2f)
                {
                    index++;
                    curTime = 0;
                    dialogTextController.OneOne(word,index, bubbleText,ref isChat);
                }
                curTime += Time.unscaledDeltaTime;
            }
            else
            {
                index = 0;
                curTime = 0;
            }
        }

        private void Show(DialogEntity obj, bool show)
        {
            bubbleText.text = "";
            isChat = true;
            word = obj.BubbleWord();
            word = dialogTextController.IsExchangeText(word, "`", ",");

            if (speechBubble != null &&
                speechBubble.activeSelf == show) return;
            
            speechBubble.SetActive(show);
            if(!show) return;
            speechBubble.transform.position = obj.transform.position;
            speechBubble.transform.SetParent(obj.transform);
        }
    }
}