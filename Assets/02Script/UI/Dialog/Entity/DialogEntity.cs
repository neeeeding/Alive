using System;
using System.Collections.Generic;
using System.Threading;
using _02Script.Manager;
using _02Script.Player;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Etc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.UI.Dialog.Entity
{
    public class DialogEntity : MonoBehaviour
    {
        public static Action<DialogEntity, bool> OnCanDialog; // 말풍선 출력
        
        //저장을 num : (챕터의) 넘버/ chapter : 챕터/ text : 마지막 대화
        public static Action<DialogEntitySO,DialogEntity> OnChat;

        #region 변수들
        [Header("Setting")]
        //감지를 위한
        [SerializeField] protected LayerMask playerLayer;// 플레이어 (레이어)
        [SerializeField]protected float DialogRadius = 2.75f;   // 대화 반경
        [SerializeField]protected float SpeechBubbleRadius = 5f;   // 말풍선 반경
        
        [Header("Need")]
        //기본 설정
        [SerializeField] protected DialogEntitySO dialogEntitySo; //나는 누구인가
        [SerializeField] protected int chapter; //챕터
        [SerializeField] protected int finalNum; //번호
        
        //상태 유무
        protected bool isChat; //대화 가능 상태 유무
        protected static bool doChat; //대화중인지
        
        //아껴살려고
        protected Collider[] DailogHits = new Collider[1], SpeechBubbleHits = new Collider[1];
        protected bool doActionB; //말풍선 관련 액션 여러번 호출 안 시키려고
        protected bool doActionC; //말풍선 관련 액션 여러번 호출 안 시키려고
        
        protected CancellationTokenSource cts = new(); //시간을 위해
        
        //저장
        protected PlayerStatSC path; //스탯 (저장 공간)
        
        #endregion
        
        protected virtual void Awake()
        {
            SetChapter();
            isChat = false;
            doChat = false;
            path = GameManager.Instance.PlayerStat;
        }
        protected virtual void OnDrawGizmos() //탐지 범위 그리기
        { 
            Gizmos.color = isChat? Color.red: Color.green;
            Gizmos.DrawWireSphere(transform.position, DialogRadius);
        }

        protected virtual void Update()
        {
            
        }
   
        public virtual string BubbleWord()
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
        
        public virtual void NextChapter(int c = 1, int num = 0)
        {
            chapter += c;
            finalNum = num;
            OnCanDialog?.Invoke(this,true);
        }
        
        public virtual void EndDialog() //대화 끝
        {
            finalNum = 1;
            //chapter++;
            DoChat(false);
            isChat = false;
        }
        
        public virtual (int c, int n) CurrentDialog() //현재 진행 사항 (챕터, 넘버 값 넘겨주기)
        {
            return (chapter, finalNum);
        }
        
        public virtual void NextDialog(int i) //대화가 진행 될 때마다
        {
            finalNum = i;
        }

        public virtual void DoChat(bool isChat) //대화 중인지 판별
        {
            doChat = isChat;
        }

        protected virtual void DoDialog() //대화 하기 (상호작용)
        {
            OnChat?.Invoke(dialogEntitySo,this);
        }

        protected void SetChapter() //혹시 모를 챕터 다시 정하기
        {
            if(chapter/1000 != 0) return;
            
            int baseChapter = ((int)dialogEntitySo.EntityName/1000)*1000;
            chapter += baseChapter;
        }

        protected virtual void OnDestroy()
        {
            doChat = false;
            OnCanDialog?.Invoke(this,false);
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}
