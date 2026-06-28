using System;
using System.Collections.Generic;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Obj.Entity;
using _02Script.UI.Dialog.Entity;
using _02Script.SaveData;
using _02Script.UI.Save;
using _02Script.UI.Dialog.Etc;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using TextAsset = UnityEngine.TextAsset;

namespace _02Script.UI.Dialog.Dialog
{
    public class Dialog : MonoBehaviour
    {
        public static Action OnGame; //채팅 끝나면

        #region 변수
        [SerializeField] private TextMeshProUGUI dialogText; //대화

        [Space(20f)] [SerializeField] private DialogEntitySO[] allCharacter; //모든 캐릭터의 정보. (여럿이서 말 할 때)

        //스크립트
        [SerializeField] private DoScript doScript;
        [SerializeField] private DialogTextController dialogTextController; //텍스트 출력 관련
        [SerializeField] private DialogSelect dialogSelect; //선택지 관련
        [SerializeField] private ChatSetting setting; //세팅 해주는 거

        [SerializeField] private DialogItem dialogItem; //아이템 관련
        [SerializeField] private ItemDataSO[] allItems; //가지고 있을 아이템들
        private SerializedDictionary<ItemType, ItemDataSO> itemDictionary = new SerializedDictionary<ItemType, ItemDataSO>();

        private List<Dictionary<string, string>> dialog; //csv 대화
        [Header("Show")] [SerializeField] private int currentChapter; //현재 챕터
        [SerializeField] private int currentNum; //현재 번호
        [SerializeField] private int currentChat; //현재 CSV의 배열

        private DialogEntity _currentDialogEntity;
        private DialogEntitySO _currentSO; //정보
        [SerializeField] private DialogEntitySO chatPlayer; //말하고 있는애

        private readonly string _am = "AM_House";
        
        //글자 입력 관련
        private string _chatText; //입력해야하는 거
        private bool _isTime; //시간 재는지
        private float _curTime; //1초 시간
        private int _nCount; //현 출력한 글자 번째
        private bool _isAM; // 현재 씬

        private EntityName isError;
        #endregion

        [SerializeField] private SelectBtn[] selectTexts; //선택지 대화
        [Space(50f)] [SerializeField] private SerializedDictionary<DialogPosition, Vector2> characterPosition; //위치 지정

        public void DialogSetting(DialogEntitySO so, DialogEntity dialogEntity) //세팅 해주기
        {
            dialogText.text = "";
            _isAM = SceneManager.GetActiveScene().name == _am;

            if (_isAM)
            {
                HouseManager.Instance.PlayerStat.lastDialogEntity = dialogEntity;
                HouseManager.Instance.PlayerStat.lastSO = so;
            }
            _currentDialogEntity = dialogEntity;
            _currentSO = so;
            chatPlayer = so;
            
            (int chapter, int finalNum) nums = dialogEntity.CurrentDialog();
            currentChapter = nums.chapter;
            currentNum = nums.finalNum;
            
            dialogEntity.DoChat(true);
            dialogSelect.OffSelectText(); //선택지 텍스트 일단 다 끄기
            GetDialog();
            
            DoChat(false);
        }

        private void GetDialog() //대화 (챕터 번호) 얻기. (List)
        {
            TextAsset currentDialog = _currentSO.DialogTextFile[0];
            dialog = CSVReader.Read(currentDialog);
            //아이템 --
            int? isItem = dialogItem.IsHoldItem(dialog);
            if (isItem != null)
            {
                currentChapter = isItem.Value;
                currentNum = 1;
            }

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

        public void ClickNext() //다음으로
        {
            ClickNext(false);
        }

        private void ClickNext(bool b) //다음으로
        {
            if (!_currentDialogEntity && (int)isError / 10000 != 4)
            {
                if(!_isAM) return;
                UISettingManager.Instance.InGame();
                return;
            }

            if (DoChat(false))
            {
                if (_isAM)
                {
                    UISettingManager.Instance.CloseChat();
                    UISettingManager.Instance.InGame();   
                }
                else
                {
                    gameObject.SetActive(false);
                }
                
                _currentDialogEntity.NextChapter();
                _currentDialogEntity.EndDialog();
                _currentDialogEntity = null;
                OnGame?.Invoke();
            }
        }

        private void ClickSkip() //스킵 버튼 눌렀을 때
        {
            if(!DialogCheck(DialogType.SelectText, "")) return;
            int nextNum = int.Parse(dialog[currentChat][DialogType.SkipNum.ToString()]) - 1;
            currentChat +=
                nextNum - currentNum == 0
                    ? +1
                    : nextNum - currentNum; //다음 번호 정해주기.
            currentNum = nextNum + 1;
            DoChat(false);
        }

        private void RenewalText(string final) // 마지막 텍스트 갱신 (주석)
        {
            if(!_isAM) return;
            
            HouseManager.Instance.PlayerStat.lastText =
                $"{EnumToString.Name(_currentSO.EntityName)} : {final}"; //마지막 텍스트

            //해당 캐릭터 갱신 (저장 stat)
            PlayerStatSC path = HouseManager.Instance.PlayerStat;

            if (path != null)
            {
                // [수정] 챕터와 번호가 반대로 저장되던 버그 수정
                path.characterLastText[chatPlayer.EntityName][DialogType.Chapter] = currentChapter.ToString();
                path.characterLastText[chatPlayer.EntityName][DialogType.Num] = currentNum.ToString();
            }
        }

        private void Update()
        {
            if (_isTime)
            {
                if (_curTime > 0.1f)
                {
                    _nCount++;
                    _curTime = 0;
                    dialogTextController.OneOne(_chatText, _nCount, dialogText, ref _isTime);
                }
                _curTime += Time.unscaledDeltaTime;
            }
            else
            {
                _nCount = 0;
                _curTime = 0;
            }
        }

        //true : 선택지를 클릭한 상태로 넘어옴, false : 일반적인 대화
        private bool DoChat(bool isSelect) //대화(실질적인 랜더러)
        {
            if (_nCount != 0)
            {
                dialogText.text = _chatText;
                _isTime = false;
                return false;
            }

            if (currentChat > dialog.Count - 1) return true;

            if (!DialogCheck(DialogType.Do, ""))
                doScript.DoCheck(dialog[currentChat][DialogType.Do.ToString()], _currentSO); //스크립트 실행

            if ((int)isError / 10000 == 4) //오류들 해결 -------------------------------------------------------------
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
                    //혹시 모르니 현재 대사 출력
                    dialogText.text = dialogTextController.IsExchangeText(dialog[currentChat][DialogType.Text.ToString()], "`", ",");
                    return true;
                }
            }

            chatPlayer = setting.PlayerSelect(allCharacter, dialog[currentChat]); //세팅하기 (자신 so 찾기)

            if (chatPlayer.EntityName == EntityName.lie)
            {
                setting.CurrentCharacter(chatPlayer); //재 세팅
                _chatText = "";
                dialogText.text = _chatText;
            }
            
            //텍스트 출력(+ 오류X)
            if ((int)chatPlayer.EntityName / 10000 != 4)
                setting.CurrentCharacter(chatPlayer); //재 세팅

            _chatText = dialogTextController.IsExchangeText(
                dialog[currentChat][DialogType.Text.ToString()], "`", ","); //변환 해주고 원했던 대화
            _isTime = true;
            //------------------------
            
            //얻기 & 스탯 증가
            dialogItem.GetOrThrowItem(dialog[currentChat], itemDictionary);
            //------------------------

            if (_currentDialogEntity != null)
                _currentDialogEntity.NextDialog(currentNum);
            dialogSelect.HaveSelect(currentChat, currentChapter, dialog);

            if (isSelect || (int)isError / 10000 == 4) return false;
            if (!DialogCheck(DialogType.NextNum, "")) // 다음 번호가 안 비어 있다면.
            {
                int nextNum = int.Parse(dialog[currentChat][DialogType.NextNum.ToString()]);
                currentChat +=
                    nextNum - currentNum == 0
                        ? isSelect ? 0 : +1
                        : nextNum - currentNum; //다음 번호 정해주기.
                currentNum = nextNum;
            }
            return false;
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
            currentChat += selectNum;
            //얻기 & 스탯 증가
            dialogItem.GetOrThrowItem(dialog[currentChat], itemDictionary);
            //------------------------
            dialogSelect.SelectChat(selectNum, ref currentNum, ref currentChat, dialog);
            DoChat(true);
        }
        
        private void LoveUp(int i) //신뢰도 오르거나 내리는 거 있으면 해주기.
        {
            if(_isAM) return;
            if (dialog[i][DialogType.GetLove.ToString()] != "") //신뢰도 얻는게 있다면. (혹은 뺏는거)
            {
                int value = int.Parse(dialog[i][DialogType.GetLove.ToString()]);
                HouseManager.Instance.SetLove(_currentSO, value);
            }
        }
        
        public void Load() //로드 될 때
        {
            if(SceneManager.GetActiveScene().name != "AM_House" && HouseManager.Instance.PlayerStat.isChat)
                DialogSetting(_currentSO, _currentDialogEntity);
        }

        private void SetItemDictionary()
        {
            foreach (ItemDataSO item in allItems)
            {
                itemDictionary.Add(item.itemType, item);
            }
        }

        #region EnDi
        private void OnEnable()
        {
            SetItemDictionary();
            if (_isAM)
            {
                UISettingManager.Instance.InGame();   
            }
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

    public enum DialogType
    {
        Bubble, //말풍선
        Item, //아이템 종류
        GetLove, //얻는 신뢰도
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
        Do, //스크립트 실행

        //저장을 위한 (캐릭터 카드)
        Memo, //메모
        Love, //신뢰도
    }

    public enum DialogPosition
    {
        none,
        right,
        left,
        middle
    }
}
