using System.Threading.Tasks;
using _02Script.Etc;
using _02Script.Manager;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Save;
using UnityEngine;

namespace _02Script.UI.Dialog.Entity
{
    public class Character : DialogEntity
    {
        private void Start()
        {
            _ = SpeechBubble();
        }

        private void Load() //로드 될 때
        {
            path = GameManager.Instance.PlayerStat;

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

        private async Task  SpeechBubble()
        {
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    await AsyncTime.WaitSeconds(Random.Range(1,5), cts.Token);
                    OnCanDialog?.Invoke(this, true);
                }
                catch (TaskCanceledException){break;}
                try
                {
                    await AsyncTime.WaitSeconds(3f, cts.Token);
                    OnCanDialog?.Invoke(this, false);
                }
                catch (TaskCanceledException){break;}
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
            //플레이어와 관해서 수정 요함.
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
    }
}
