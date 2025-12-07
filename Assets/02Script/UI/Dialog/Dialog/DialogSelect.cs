using System.Collections.Generic;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.Dialog.Etc;
using UnityEngine;

namespace _02Script.UI.Dialog.Dialog
{
    public class DialogSelect : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private DialogInputSO inputSo;
        [SerializeField] private SelectBtn[] selectTexts; //선택지 대화
        [SerializeField] private DialogTextController dialogTextController; //텍스트 출력 관련
        
        public void HaveSelect(int i, int currentChapter,
        List<Dictionary<string,string>> dialog,
        DialogEntitySO chatPlayer) //선택지가 있는지 (있으면 개수만 큼 세팅.)
        {
            OffSelectText(); //일단 다 끄기

            if (dialog[i][DialogType.Select.ToString()] == "" && chatPlayer.EntityName != EntityName.lie) //선택지가 없다면
                return;
            
            if (chatPlayer.EntityName == EntityName.lie)
            {
                PlayerNeed(i, dialog);
            }
            else
            {
                string[] all = dialog[i][DialogType.Select.ToString()].Split('~');
                
                int[] nums = new int[all.Length];
                for (int j = 0; j < all.Length; j++)
                {
                    nums[j] = int.Parse(all[j]);
                }
                
                OnlySelect(i,nums, dialog, currentChapter);
            }
           
        }

        private void OnlySelect(int i,int[] count, List<Dictionary<string,string>> dialog, int currentChapter) //실재 선택용
        {
            for(int j = 0; j < count.Length && j < selectTexts.Length; j++) //반복문
            {
                selectTexts[j].gameObject.SetActive(true);

                int selectNumText = count[j]; //선택지에 사용될 텍스트 (번호) 찾기

                int selectNum = selectNumText;
                for (int ii = 0; ii < dialog.Count - 1; ii++) //맞는 거 찾기
                {
                    if (dialog[ii][DialogType.Chapter.ToString()] == currentChapter.ToString()
                        && dialog[ii][DialogType.Num.ToString()] == selectNumText.ToString())
                    {
                        selectNum = ii;
                        break;
                    }
                }
                
                string text = dialogTextController.IsExchangeText(
                    dialog[selectNum][DialogType.Text.ToString()], "`", ",");

                selectTexts[j].SetSelect(text, j); //선택 그거 세팅 해주기.
            }
        }

        private void PlayerNeed(int selectNumText,List<Dictionary<string,string>> dialog) //플레이어 용
        {
            selectTexts[0].gameObject.SetActive(true);

            string text = dialogTextController.IsExchangeText(
                dialog[selectNumText][DialogType.Text.ToString()], "`", ",");

            selectTexts[0].SetSelect(text, -1); //선택 그거 세팅 해주기.
            
        }
        
        public void SelectChat(int selectNum, ref int currentNum, ref int currentChat,
            List<Dictionary<string,string>> dialog) //선택된 선택지가 있을 시 (대화 계속 진행)
        {
            selectNum += currentNum;
            
            currentChat += selectNum - currentNum; //선택된 번호로 바꿔주기
            currentNum = selectNum;
            int nextNum = int.Parse(dialog[currentChat][DialogType.NextNum.ToString()]);
            
            //다음 번호로 바꿔 주기 (선택된 문항을 말할 순 없으니까. (사실 호불호긴 해.))
            currentChat += nextNum - currentNum == 0 ?
                +0 : nextNum - currentNum;
            
            currentNum = nextNum;
            currentNum = nextNum;

            OffSelectText(); //다 꺼주기
        }

        public void OffSelectText() // 선택지 텍스트 다 꺼주기
        {
            foreach (var item in selectTexts)
            {
                item.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            SetBtnManager();
        }

        private void SetBtnManager()
        {
            foreach (var s in selectTexts)
            {
                s.SetInputSO(inputSo);
            }
        }
    }
}