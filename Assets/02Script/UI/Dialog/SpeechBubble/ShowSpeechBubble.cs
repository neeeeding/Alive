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
        
        private string _word; //내용
        private int _index; //단어 수
        private bool _isChat; //출력 하는 중인지
        private float _curTime;
        
        private void OnEnable()
        {
            _word =  "......";
            speechBubble.SetActive(false);
            DialogEntity.OnCanDialog += Show;
        }

        private void OnDisable()
        {
            DialogEntity.OnCanDialog -= Show;
        }

        private void Update()
        {
            if (_isChat)
            {
                if (_curTime > 0.2f)
                {
                    _index++;
                    _curTime = 0;
                    dialogTextController.OneOne(_word,_index, bubbleText,ref _isChat);
                }
                _curTime += Time.unscaledDeltaTime;
            }
            else
            {
                _index = 0;
                _curTime = 0;
            }
        }

        private void Show(DialogEntity obj, bool show)
        {
            bubbleText.text = "";
            _isChat = true;
            _word = obj.BubbleWord();
            _word = dialogTextController.IsExchangeText(_word, "`", ",");

            if (speechBubble != null &&
                speechBubble.activeSelf == show) return;
            
            speechBubble.SetActive(show);
            if(!show) return;
            speechBubble.transform.position = obj.transform.position;
            speechBubble.transform.SetParent(obj.transform);
        }
    }
}