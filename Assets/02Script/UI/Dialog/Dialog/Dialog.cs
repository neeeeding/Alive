using System;
using System.Collections.Generic;
using _02Script.Item;
using _02Script.Manager;
using _02Script.UI.Dialog.Entity;
using _02Script.Player;
using _02Script.UI.Save;
using _02Script.UI.Dialog.Etc;
using TMPro;
using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace _02Script.UI.Dialog.Dialog
{
    public class Dialog : MonoBehaviour
    {
        public static Action OnGame; //채팅 끝나면


        #region 변수

        [Header("Need")] [SerializeField] private GameObject dialogUI;
        [SerializeField] private ChatSetting setting; //세팅 해주는 거
        [SerializeField] private TextMeshProUGUI dialogText; //대화

        [Space(20f)] [SerializeField] private DialogEntitySO[] allCharacter; //모든 캐릭터의 정보. (여럿이서 말 할 때)

        //스크립트
        [SerializeField] private DoScript doScript;
        [SerializeField] private DialogTextController dialogTextController; //텍스트 출력 관련
        [SerializeField] private DialogSelect dialogSelect; //선택지 관련

        [SerializeField] private DialogItem dialogItem; //아이템 관련
        // [SerializeField] private InventoryManager manager; //아이템 관련
        // [SerializeField] private SerializedDictionary<string, ItemDataSO> items; //가지고 있을 아이템들

        private List<Dictionary<string, string>> dialog; //csv 대화
        [Header("Show")] [SerializeField] private int currentChapter; //현재 챕터
        [SerializeField] private int currentNum; //현재 번호
        [SerializeField] private int currentChat; //현재 CSV의 배열

        private DialogEntity _currentDialogEntity;
        private DialogEntitySO _currentSO; //정보
        [SerializeField] private DialogEntitySO chatPlayer; //말하고 있는애

        //글자 입력 관련
        private string chatText; //입력해야하는 거
        private bool isTime; //시간 재는지
        private float curTime; //1초 시간
        private int nCount; //현 출력한 글자 번째

        private EntityName isError;

        #endregion

        [SerializeField] private SelectBtn[] selectTexts; //선택지 대화
        [Space(50f)] [SerializeField] private Dictionary<DialogPosition, Vector2> characterPosition; //위치 지정

        private bool holdItem; //true : 아이템 들고 있음, false : 아이템 없음

        public void DialogSetting(DialogEntitySO so, DialogEntity dialogEntity) //세팅 해주기
        {
            GameManager.Instance.PlayerStat.lastDialogEntity = dialogEntity;
            GameManager.Instance.PlayerStat.lastSO = so;
            holdItem = false;
            _currentDialogEntity = dialogEntity;
            _currentSO = so;
            int[] nums = dialogEntity.CurrentDialog();
            currentChapter = nums[0];
            currentNum = nums[1];
            chatPlayer = so;
            GetDialog();
            DoChat();
        }

        private void GetDialog() //대화 (챕터 번호) 얻기. (List)
        {
            TextAsset currentDialog = _currentSO.DialogTextFile[0];
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
            if (dialog[currentChat][DialogType.Select.ToString()].ToString() != "")
            {
                if (DoChat())
                {
                    _currentDialogEntity.NextChapter();
                    UISettingManager.Instance.CloseChat();
                    OnGame?.Invoke();
                }
                else
                    _currentDialogEntity.NextDialog(currentNum);
            }
        }

        private void ClickSkip() //스킵 버튼 눌렀을 때
        {
            int nextNum = int.Parse(dialog[currentChat][DialogType.SkipNum.ToString()]) - 1;
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
                $"{ChatSetting.Name(_currentSO.EntityName)} : {final}"; //마지막 텍스트

            //해당 캐릭터 갱신 (저장 stat)
            PlayerStatSC path = GameManager.Instance.PlayerStat;

            if (path != null)
            {
                //저장
                path.characterLastText[chatPlayer.EntityName][DialogType.Chapter] = currentNum.ToString();
                path.characterLastText[chatPlayer.EntityName][DialogType.Num] = currentChapter.ToString();
                //path.characterlastText[chatPlayer.characterName][DialogType.Text] = final;
            }
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
                        currentChapter = int.Parse(dialog[i][DialogType.Chapter.ToString()]);
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
                int nextNum = int.Parse(dialog[currentChat][DialogType.NextNum.ToString()]) - 1;
                currentChat +=
                    nextNum - currentNum == 0
                        ? +1
                        : nextNum - currentNum; //다음 번호 정해주기. (마지막이 본인이면 1추가로 나가게 해버리기.(대화 자체는 줄어버림.(???)))
                currentNum = nextNum + 1;
            }

            RenewalText(chatText); //마지막 텍스트 갱신
            return getOut;
        }
        



        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        private void DialogSetting(DialogEntitySO so, DialogEntity dialogEntity) //세팅 해주기
        {
            dialogText.text = "";
            dialogUI.SetActive(true);
            _currentSO = so;
            //items = so.items;

            _currentDialogEntity = dialogEntity;
            dialogEntity.DoChat(true);
            (int c, int n) nums = dialogEntity.CurrentDialog();
            currentChapter = nums.c;
            currentNum = nums.n;

            dialogSelect.OffSelectText(); //선택지 텍스트 일단 다 끄기
            GetDialog();

            if (_currentDialogEntity as Island) //다른 애들은 F처리로 알아서 실행 되니.
                DoChat(false);
        }

        private void GetDialog() //대화 (챕터 번호) 얻기. (List)
        {
            TextAsset currentDialog = _currentSo.DialogTextFile[0];
            dialog = CSVReader.Read(currentDialog);

            //CSV 배열 찾기
            for (int i = 0; i < dialog.Count - 1; i++)
            {
                if (DialogCheck(DialogType.Chapter, currentChapter.ToString(), i)
                    && DialogCheck(DialogType.Num, currentNum.ToString(), i)) //해당 배열의 수가 챕터랑 번호가 같으면
                {
                    currentChat = i; //해당 배열의 수
                    break;
                }
            }
        }

        private void Update()
        {
            if (isTime)
            {
                if (curTime > 0.2f)
                {
                    nCount++;
                    curTime = 0;
                    dialogTextController.OneOne(chatText, nCount, dialogText, ref isTime);
                }

                curTime += Time.unscaledDeltaTime;
            }
            else
            {
                nCount = 0;
                curTime = 0;
            }
        }

        private void ClickNext(bool b) //다음으로
        {
            if (!_currentDialogEntity && (int)isError / 1000 != 4)
            {
                dialogUI.SetActive(false);
                return;
            }

            if (DoChat(false))
            {
                dialogUI.SetActive(false);
                _currentDialogEntity.EndDialog();
                _currentDialogEntity = null;
                items = null;
            }
        }

        private bool DoChat(bool isSelect) //대화(실질적인 랜더러)
        {
            if (nCount != 0)
            {
                dialogText.text = chatText;
                isTime = false;
                return false;
            }

            if (currentChat > dialog.Count - 1) return true;

            if (!DialogCheck(DialogType.Do, ""))
                doScript.DoCheck(dialog[currentChat][DialogType.Do.ToString()], _currentDialogEntity); //스크립트 실행

            if ((int)isError / 1000 == 4) //오류들 해결 -------------------------------------------------------------
            {
                currentChapter = (int)isError;
                currentNum = 1;
                for (int i = 0; i < dialog.Count; i++)
                {
                    if (DialogCheck(DialogType.Chapter, currentChapter.ToString(), i)
                        && DialogCheck(DialogType.Num, currentNum.ToString(), i))
                    {
                        currentChat = i; //해당 배열의 수
                        break;
                    }
                }

                isError = EntityName.None;
            }
            else
            {
                //대화가 존재하는지 (배열 확인)
                if (!(dialog.Count > currentChat
                      && DialogCheck(DialogType.Chapter, currentChapter.ToString())
                      && DialogCheck(DialogType.Num, currentNum.ToString()))) //해당 배열의 수가 챕터랑 번호가 같으면
                {
                    return true;
                }
            }

            chatPlayer = setting.PlayerSelect(currentChat, allCharacter, dialog); //새팅하기 (자신 so 찾기)

            if (chatPlayer.EntityName != EntityName.Me) //플레이어가 아니면 (+ 오류X)
            {
                if ((int)chatPlayer.EntityName / 1000 != 4)
                    setting.CurrentCharacter(chatPlayer); //재 세팅

                chatText = dialogTextController.IsExchangeText(
                    dialog[currentChat][DialogType.Text.ToString()], "`", ","); //변환 해주고 원했던 대화
                isTime = true;
            }
            else if (chatPlayer.EntityName == EntityName.Me)
            {
                setting.CurrentCharacter(chatPlayer); //재 세팅
                chatText = "";
                dialogText.text = chatText;
            }

            if (_currentDialogEntity != null)
                _currentDialogEntity.NextDialog(currentNum);
            dialogSelect.HaveSelect(currentChat, currentChapter, dialog, chatPlayer);

            CheckMoney(); //돈

            if (isSelect || (int)isError / 1000 == 4) return false;
            if (!DialogCheck(DialogType.NextNum, "")) // 다음 번호가 안 비어 있다면.
            {
                int nextNum = int.Parse(dialog[currentChat][DialogType.NextNum.ToString()]);
                currentChat +=
                    nextNum - currentNum == 0
                        ? isSelect ? 0 : +1
                        : nextNum - currentNum; //다음 번호 정해주기. (마지막이 본인이면 1추가로 나가게 해버리기.(대화 자체는 줄어버림.(???)))
                currentNum = nextNum;
            }

            return false;
        }

        private void CheckMoney() //돈과 아이템
        {
            if (!dialogMoney.UseMoney(currentChat, dialog, moneyManager)) isError = EntityName.NoMoney; //돈

            if (!dialogItem.HaveItem(currentChat, dialog, items, manager)) isError = EntityName.NoItem;
        }

        private bool DialogCheck(DialogType key, string check, int? i = null) // 찾기
        {
            i = i ?? currentChat;
            if (i > dialog.Count - 1) return false;

            if (!dialog[i ?? currentChat].ContainsKey(key.ToString())) return false;
            return (dialog[i ?? currentChat][key.ToString()] == check);
        }

        private void SelectChat(int selectNum) //선택
        {
            dialogText.text = "";
            dialogSelect.SelectChat(selectNum, ref currentNum, ref currentChat, dialog);
            DoChat(true);
        }
        
        
        
        private void LoveUP(int i) //호감도 오르거나 내리는 거 있으면 해주기.
        {
            if (dialog[i][DialogType.GetLove.ToString()].ToString() != "") //호감도 얻는게 있다면. (혹은 뺏는거)
            {
                int value = int.Parse(dialog[i][DialogType.GetLove.ToString()]);
                GameManager.Instance.SetLove(_currentSO, value); //여러명 일 때 만약 주체가 아닌 다른 이 라면.
            }
        }
        
        public void Load() //로드 될 때
        {
            DialogSetting(_currentSO, _currentDialogEntity);
        }

        #region EnDi

        private void OnEnable()
        {
            dialogUI.SetActive(false);
            SelectBtn.OnSelect += SelectChat;
            DialogEntity.OnChat += DialogSetting;
            PlayerDialogInput.OnChat += ClickNext;
            ChatBtn.OnSkipChat += ClickSkip;
            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            SelectBtn.OnSelect -= SelectChat;
            DialogEntity.OnChat -= DialogSetting;
            PlayerDialogInput.OnChat -= ClickNext;
            ChatBtn.OnSkipChat -= ClickSkip;
            LoadCard.OnLoad -= Load;
        }

        #endregion
    }
}

public enum DialogType
    {
        Bubble, //말풍선
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
        
        GetItem, //상호작용 하는 아이템
        ItemCount, // 개수
        UseMoney, //돈
        Do, //스크립트 실행

        //저장을 위한 (캐릭터 카드)
        Memo, //메모
        Love, //호감도
    }

    public enum DialogPosition
    {
        none,
        right,
        left,
        middle
    }
}
