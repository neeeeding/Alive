using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using _02Script.Manager;
using _02Script.Player;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using _02Script.UI.Save;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.UI.Dialog.Entity
{
    public class DialogEntity : MonoBehaviour
    {
        public static Action<DialogEntity, bool> OnCanDialog; // 말풍선 출력
        //저장을 num : (챕터의) 넘버/ chapter : 챕터/ text : 마지막 대화
        //LikeabilityCard = 
        public static Action<DialogEntitySO,DialogEntity> OnChat;

        [SerializeField] private DialogEntitySO dialogEntitySo;
        [SerializeField] private int chapter; //챕터
        [SerializeField] private int finalNum; //번호
        private PlayerStatSC path; //스탯 (저장 공간)
        private bool isChat;
        
        CancellationTokenSource cts = new(); //시간을 위해

        private void Awake()
        {
            isChat = false;
            path = GameManager.Instance.PlayerStat;
        }
        private async void Start()
        {
            await SpeechBubble();
        }
        
        public string BubbleWord()
        {
            TextAsset currentDialog = dialogEntitySo.DialogTextFile[0];
            List<Dictionary<string,string>> dialog = CSVReader.Read(currentDialog);

            List<string> words =  new List<string>();
            
            for (int i = 0; i < dialog.Count; i++)
            {
                if (dialog[i][DialogType.Bubble.ToString()] != "") // 다음 번호가 안 비어 있다면.
                {
                    string num = dialog[i][DialogType.Bubble.ToString()];
                    if (num == chapter.ToString())
                    {
                        words.Add(dialog[i][DialogType.Text.ToString()]);
                    }
                }
            }

            if (words.Count > 0)
            {
                return words[Random.Range(0, words.Count)];
            }
            
            return ".......";
        }
        
        public void DoChat(bool isChat) //대화 중인지 판별
        {
            doChat = isChat;
        }

        public void Load() //로드 될 때
        {
            path = GameManager.Instance.PlayerStat; // 

            if (path != null)
            {
                int.TryParse(path.characterLastText[dialogEntitySo.EntityName][DialogType.Chapter], out chapter);
                int.TryParse(path.characterLastText[dialogEntitySo.EntityName][DialogType.Num], out finalNum);
            }

            //아이템이나 특수 대화에서는 문제가 없는지 확인 할 것
            if (GameManager.Instance.PlayerStat.isChat)
            {
                UISettingManager.Instance.CloseChat();
                finalNum--; //대화를 시작 할 때 1를 추가하고 시작함으로.
                UISettingManager.Instance.Chat(GameManager.Instance.PlayerStat.lastSO, GameManager.Instance.PlayerStat.lastDialogEntity);
            }
            else
            {
                UISettingManager.Instance.CloseChat();
            }
        }

        public void NextDialog(int i) //대화가 진행 될 때마다
        {
            finalNum = i;

            path.characterLastText[dialogEntitySo.EntityName][DialogType.Num] = finalNum.ToString();
        }

        public int[] CurrentDialog() //현재 진행 사항 (챕터, 넘버 값 넘겨주기)
        {
            return new int[]{chapter, finalNum};
        }

        private async Task  SpeechBubble()
        {
            try
            {
                await AsyncTime.WaitSeconds(30f, cts.Token);
                OnCanDialog?.Invoke(this, true);
            }
            catch (TaskCanceledException)
            {
            }
            try
            {
                await AsyncTime.WaitSeconds(5f, cts.Token);
                OnCanDialog?.Invoke(this, false);
            }
            catch (TaskCanceledException)
            {
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                isChat = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isChat = false;
            }
        }

        public void NextChapter() //다음 챕터로 설정 해주기
        {
            finalNum = 1;
            chapter++;
            
            
            int baseChapter = ((int)dialogEntitySo.EntityName/1000)*1000;
            chapter += baseChapter;

            path.characterLastText[dialogEntitySo.EntityName][DialogType.Chapter] = chapter.ToString();
            path.characterLastText[dialogEntitySo.EntityName][DialogType.Num] = finalNum.ToString();
        }

        public void ClickCharacter() //대화 하기 (클릭)
        {
            if (isChat)
            {
                int.TryParse(path.characterLastText[dialogEntitySo.EntityName][DialogType.Chapter],out chapter);
                int.TryParse(path.characterLastText[dialogEntitySo.EntityName][DialogType.Num],out finalNum);
                //finallNum = 1;
                OnChat?.Invoke(dialogEntitySo,this);
            }
        }

        private void OnEnable()
        {
            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            LoadCard.OnLoad -= Load;
        }
        protected virtual void OnDestroy()
        {
            OnCanDialog?.Invoke(this,false);
            cts.Cancel();
        }
    }
}
