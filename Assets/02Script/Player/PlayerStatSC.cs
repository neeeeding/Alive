using System;
using _02Script.Item;
using _02Script.Etc;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using UnityEngine;

namespace _02Script.Player
{
    [Serializable]
    public class PlayerStatSC
    {
        public string sceneName; //활동 하던 씬이름

        public bool isChat; //ture : 채팅 중, false : 인 게임

        public int playerCoin; //소지금

        public Vector2 playerPosition; //플레이어 위치

        [Space(50f)]
        public DialogEntity lastDialogEntity; //마지막 캐릭터
        public DialogEntitySO lastSO;
        public string lastText; //마지막 대화

        public SaveDictionary<EntityName, SaveDictionary<DialogType, string>> characterLastText; //캐릭터 마지막 대화 이름<다이얼로그(종류), 번째(혹은 텍스트)>

        [Space(50f)] //날짜
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;

        public SaveDictionary<ItemCategory, SaveDictionary<ItemType, int>> items; //아이템들 카테고리<종류,수>
        
        [ContextMenu("ResetStat")]
        public void ResetStat()
        {
            sceneName = "TestOutSide";
            isChat = false;

            playerCoin = 5;
            playerPosition = new Vector2(0, 0);

            lastText = "마지막 대화가 없습니다.";
            
            ResetCharacter();

            year = 2000;
            month = 1;
            day = 1;
            hour = 1;
            minute = 0;
             
            ResetItem();
        }
        
        public void ResetCharacter() //캐릭터들  전부 초기화
        {
            characterLastText = new SaveDictionary<EntityName, SaveDictionary<DialogType, string>>();
            characterLastText.Clear();

            int num;

            foreach (EntityName name in Enum.GetValues(typeof(EntityName))) //이름들 저장
            {
                num = (int)name / 1000;
                SaveDictionary<DialogType, string> di = new SaveDictionary<DialogType, string>();

                foreach (DialogType dialog in
                         Enum.GetValues(typeof(DialogType))) //모든 걸 저장 / 다이얼로그 종류 (챕터, 넘버, 텍스트, 메모, 러브 만 사용하긴 함.)
                {
                    di.Add(dialog, ""); // " " 초기화
                }

                characterLastText.Add(name, di); //저장
            }
        }
        
        public void ResetItem() //스탯의 아이템 전부 초기화
        {
            items = new SaveDictionary<ItemCategory, SaveDictionary<ItemType, int>>();
            items.Clear();

            int num;

            foreach (ItemCategory category in Enum.GetValues(typeof(ItemCategory))) //카테고리 저장
            {
                if (category == ItemCategory.coin || category == ItemCategory.none) //코인 제외
                    continue;

                num = (int)category / 1000;
                SaveDictionary<ItemType, int> item = new SaveDictionary<ItemType, int>(); //아이템

                foreach (ItemType type in Enum.GetValues(typeof(ItemType))) //해당 카테고리와 앞 자리 같은 종료를 저장
                {
                    if (num != (int)type / 1000) //앞자리 비교
                        continue;

                    item.Add(type, 0); //0으로 초기화
                }

                items.Add(category, item); //저장
            }
        }
    }
}
