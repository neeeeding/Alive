using System;
using System.Collections.Generic;
using _02Script.Item;
using _02Script.Manager;
using _02Script.Obj.Character;
using _02Script.Player;
using _02Script.UI.Save;
using _02Script.Etc;
using TMPro;
using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace _02Script.UI.Chat
{
    public class Dialog : MonoBehaviour
    {
        public static Action OnGame; //채팅 끝나면

        [Header("Component")] [SerializeField] private ChatSetting setting; //세팅 해주는 거
        [SerializeField] private TextMeshProUGUI dialogText; //대화
        [SerializeField] private SelectBtn[] selectTexts; //선택지 대화
        [Space(20f)] [SerializeField] private CharacterSO[] allCharacter; //모든 캐릭터의 정보. (호감도 못 올림)
        [Space(50f)] [SerializeField] private Dictionary<DialogPosition, Vector2> characterPosition; //위치 지정

        [Header("CurrentChapter")] private List<Dictionary<string, object>> dialog; //csv 대화
        [SerializeField] private int currentChapter; //현재 챕터
        [SerializeField] private int currentNum; //현재 번호
        [SerializeField] private int currentChat; //현재 CSV의 배열

        private Character currentCharacter; //다음 대화임을 알려주려고
        private CharacterSO currentSO; //정보
        [SerializeField] private CharacterSO chatPlayer; //말하고 있는애

        private bool holdItem; //true : 아이템 들고 있음, false : 아이템 없음

        public void DialogSetting(CharacterSO so, Character character) //세팅 해주기
        {
            GameManager.Instance.PlayerStat.lastCharacter = character;
            GameManager.Instance.PlayerStat.lastSO = so;
            holdItem = false;
            currentCharacter = character;
            currentSO = so;
            int[] nums = character.CurrentDialog();
            currentChapter = nums[0];
            currentNum = nums[1];
            OffSelectText(); //선택지 텍스트 일단 다 끄기
            GetDialog();
            DoChat();
        }

        private void GetDialog() //대화 (챕터 번호) 얻기. (List)
        {
            TextAsset currentDialog = currentSO.characterDialog[0];
            dialog = CSVReader.Read(currentDialog);
            IsHoldItem();

            //CSV 배열 찾기
            for (int i = 0; i < dialog.Count - 1; i++)
            {
                if (dialog[i][DialogType.Chapter.ToString()].ToString() == currentChapter.ToString()
                    && dialog[i][DialogType.Num.ToString()].ToString() == currentNum.ToString()) //해당 배열의 수가 챕터랑 번호가 같으면
                {
                    currentChat = i; //해당 배열의 수
                    break;
                }
            }
        }

        public void ClickNext() //다음으로
        {
            if (!HaveSelect(currentChat))
            {
                if (DoChat())
                {
                    currentCharacter.NextChapter();
                    UISettingManager.Instance.CloseChat();
                    OnGame?.Invoke();
                }
                else
                    currentCharacter.NextDialog(currentNum);
            }
        }

        private void ClickSkip() //스킵 버튼 눌렀을 때
        {
            int nextNum = (int)dialog[currentChat][DialogType.SkipNum.ToString()] - 1;
            currentChat +=
                nextNum - currentNum == 0
                    ? +1
                    : nextNum - currentNum; //다음 번호 정해주기. (마지막이 본인이면 1추가로 나가게 해버리기.(대화 자체는 줄어버림.(???)))
            currentNum = nextNum + 1;
            DoChat();
        }

        private void RenewalText(string final) // 마지막 텍스트 갱신
        {
            GameManager.Instance.PlayerStat.lastText =
                $"{ChatSetting.Name(currentSO.characterName)} : {final}"; //마지막 텍스트

            //해당 캐릭터 갱신 (저장 stat)
            PlayerStatSC path = GameManager.Instance.PlayerStat;

            if (path != null)
            {
                //저장
                path.characterlastText[chatPlayer.characterName][DialogType.Chapter] = currentNum.ToString();
                path.characterlastText[chatPlayer.characterName][DialogType.Num] = currentChapter.ToString();
                //path.characterlastText[chatPlayer.characterName][DialogType.Text] = final;
            }
        }

        private void SelectChat(int selectNum) //선택된 선택지가 있을 시 (대화 계속 진행)
        {
            currentChat += selectNum - currentNum; //선택된 번호로 바꿔주기
            currentNum = selectNum;

            //다음 번호로 바꿔 주기 (선택된 문항을 말할 순 없으니까. (사실 호불호긴 해.))
            int nextNum = (int)dialog[currentChat][DialogType.NextNum.ToString()];
            currentChat +=
                nextNum - currentNum == 0 ? +1 : nextNum - currentNum; //다음 번호 정해주기. (마지막이 본인이면 1추가로 나가게 해버리기.)
            currentNum = nextNum;

            OffSelectText(); //다 꺼주기
            DoChat();
        }

        private void IsHoldItem() //들고 있는 아이템 있다면 (챕터 번호)
        {
            ItemSO so = GameManager.Instance.Item;
            if (so != null)
            {
                holdItem = true;
                for (int i = 0; i < dialog.Count - 1; i++)
                {
                    if (dialog[i][DialogType.Item.ToString()].ToString() ==
                        so.itemType.ToString()) //대화의 아이템 창과 들고 있는 아이템 찾기
                    {
                        currentChapter = (int)dialog[i][DialogType.Chapter.ToString()];
                        GameManager.Instance.AddItemCount(so.category, so.itemType, -1); //아이템 빼기
                        break;
                    }
                }
            }
        }

        private bool DoChat() //대화(실질적인 랜더러)
        {
            bool getOut = true;

            //대화가 존재하는지 (배열 확인)
            if (dialog[currentChat][DialogType.Chapter.ToString()].ToString() == currentChapter.ToString()
                && dialog[currentChat][DialogType.Num.ToString()].ToString() ==
                currentNum.ToString()) //해당 배열의 수가 챕터랑 번호가 같으면
            {
                getOut = false;
                PlayerSelect(currentChat);
                LoveUP(currentChat);
            }
            else
                getOut = true;

            setting.CurrentCharacter(chatPlayer); //세팅 해주는 거
            string chatText =
                IsExchangeText(dialog[currentChat][DialogType.Text.ToString()].ToString(), "`", ","); //변환 해주고 원했던 대화
            dialogText.text = chatText;

            if (dialog[currentChat][DialogType.NextNum.ToString()].ToString() != "") // 다음 번호가 안 비어 있다면.
            {
                int nextNum = (int)dialog[currentChat][DialogType.NextNum.ToString()] - 1;
                currentChat +=
                    nextNum - currentNum == 0
                        ? +1
                        : nextNum - currentNum; //다음 번호 정해주기. (마지막이 본인이면 1추가로 나가게 해버리기.(대화 자체는 줄어버림.(???)))
                currentNum = nextNum + 1;
            }

            RenewalText(chatText); //마지막 텍스트 갱신
            return getOut;
        }

        private string IsExchangeText(string dial, string text, string change) //어떤 문자를 다른 문자로 바꾸기 ( ` => , )
        {
            string changeDialog = "";

            if (dial.Contains(text)) //바꿀것이 포함 되어있는지
            {
                string[] saveText = dial.Split(text); //일단 쪼개주기
                for (int i = 0; i < saveText.Length - 1; i++) //마지막은 미포함이니 빼주고 반복
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

        private void PlayerSelect(int i) //말하는 이에 따른 so(이름) 바꾸기
        {
            chatPlayer = currentSO;
            foreach (CharacterSO so in allCharacter)
            {
                if (so.characterName.ToString() == dialog[i][DialogType.Player.ToString()].ToString())
                {
                    chatPlayer = so;
                }
            }
        }

        private void LoveUP(int i) //호감도 오르거나 내리는 거 있으면 해주기.
        {
            if (dialog[i][DialogType.GetLove.ToString()].ToString() != "") //호감도 얻는게 있다면. (혹은 뺏는거)
            {
                int value = (int)dialog[i][DialogType.GetLove.ToString()];
                GameManager.Instance.SetLove(currentSO, value); //여러명 일 때 만약 주체가 아닌 다른 이 라면.
            }
        }

        private void OffSelectText() // 선택지 텍스트 다 꺼주기
        {
            foreach (var item in selectTexts)
            {
                item.gameObject.SetActive(false);
            }
        }

        private bool HaveSelect(int i) //선택지가 있는지 (있으면 개수만 큼 세팅.)
        {
            bool haveSelec = false; //false : 선택지 없음 (다음 번호)/ true : 선택지 있음 (건너뛰어서)

            OffSelectText(); //일단 다 끄기

            if (dialog[i][DialogType.Select.ToString()].ToString() != "") //선택지가 있다면
            {
                haveSelec = true;
                int count = (int)dialog[i][DialogType.Select.ToString()];
                for (int j = 0; j < count && j < selectTexts.Length; j++) //반복문
                {
                    selectTexts[j].gameObject.SetActive(true);

                    int selectNumText =
                        (int)dialog[i][DialogType.SelectText.ToString() + (j + 1)] -
                        1 /*0부터 시작하니*/; //선택지에 사용될 텍스트 (번호) 찾기
                    string text = dialog[selectNumText][DialogType.Text.ToString()].ToString();

                    selectTexts[j].SetSelect(text, selectNumText); //선택 그거 세팅 해주기.
                }
            }

            return haveSelec;
        }

        public void Load() //로드 될 때
        {
            DialogSetting(currentSO, currentCharacter);
        }

        #region EnDi

        private void OnEnable()
        {
            SelectBtn.OnSelect += SelectChat;
            ChatBtn.OnSkipChat += ClickSkip;
            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            SelectBtn.OnSelect -= SelectChat;
            ChatBtn.OnSkipChat -= ClickSkip;
            LoadCard.OnLoad -= Load;
        }

        #endregion
    }

    public enum DialogType
    {
        Select, //선택지 인지 (개수)
        ItemCategory, //아이템 카테고리
        Item, //아이템 종류
        GetLove, //얻는 호감도
        Chapter, //해당 챕터 (한 대화)
        Num, //챕터의 세부 번호
        Text, //대화
        Player, //대화 하는 캐릭터
        Position, //대화 하는 캐릭터의 위치
        OtherPosition, //이전 대화의 캐릭터의 위치
        SkipNum, //스킵 했을 때 넘어가는 번호
        NextNum, //다음으로 넘어갈 번호
        SelectText, //선택지 (개수 따라)

        //저장을 위한 (캐릭터 카드)
        Memo, //메모
        Love //호감도
    }

    public enum DialogPosition
    {
        none,
        right,
        left,
        middle
    }
}
