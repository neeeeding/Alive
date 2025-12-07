using System;
using TMPro;
using UnityEngine;

namespace _02Script.UI.Dialog.Etc
{
    public class SelectBtn : MonoBehaviour
    {
        public static Action<int> OnSelect; //선택한 번호

        private int mySelectNum; //본인의 번호 (챕터에서, 넘버)
        [SerializeField] private TextMeshProUGUI selectTexts; //선택지 대화

        private DialogInputSO _inputSo;
        public void SetSelect(string text, int thisNum) //대화 내용, 선택지 번호
        {
            selectTexts.text = text;
            mySelectNum = thisNum;
        }
        
        public void SetInputSO(DialogInputSO inputSo)
        {
            _inputSo = inputSo;
        }
        private void InputSelect(int num)
        {
            if(num == mySelectNum)
                ClickSelect();
        }

        public void ClickSelect() //선택 버튼 누를 때
        {
            OnSelect?.Invoke(mySelectNum + 1 /*0부터 시작하니*/);
        }
        
        private void OnEnable()
        {
            if(_inputSo != null)
                _inputSo.OnClickSelect += InputSelect;
        }

        private void OnDisable()
        {
            if(_inputSo != null)
                _inputSo.OnClickSelect -= InputSelect;
        }
    }
}