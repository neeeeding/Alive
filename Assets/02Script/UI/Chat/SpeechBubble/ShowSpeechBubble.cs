using _02Script.Obj.Character;
using TMPro;
using UnityEngine;

namespace _02Script.UI.Chat.SpeechBubble
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
            Character.OnCanDialog += Show;
        }

        private void OnDisable()
        {
            Character.OnCanDialog -= Show;
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

        private void Show(Character obj, bool show, bool chat)
        {
            isChat = true;
            word = chat ? obj.BubbleWord() : "......";
            word = dialogTextController.IsExchangeText(word, "`", ",");
            if (speechBubble.activeSelf == show) return;
            
            speechBubble.SetActive(show);
            if(!show) return;
            speechBubble.transform.position = obj.transform.position;
        }
    }
}