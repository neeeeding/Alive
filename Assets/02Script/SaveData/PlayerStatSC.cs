using System;
using System.Collections.Generic;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.SaveData
{
    [Serializable]
    public class PlayerStatSC
    {
        public string sceneName; //활동 하던 씬이름

        public bool isChat; //ture : 채팅 중, false : 인 게임

        public SaveDictionary<EntityName,Vector2> characterPositions; //플레이어 위치

        [Space(50f)]
        public DialogEntity lastDialogEntity; //마지막 캐릭터
        public DialogEntitySO lastSO;
        public string lastText; //마지막 대화

        public SaveDictionary<EntityName, SaveDictionary<StatsType, int>> characterStats; //캐릭터들 스탯 캐릭터<스탯 종류, 수>
        public SaveDictionary<EntityName, SaveDictionary<DialogType, string>> characterLastText; //캐릭터 마지막 대화 이름<다이얼로그(종류), 번째(혹은 텍스트)>

        [Space(50f)] //날짜
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;

        public SaveDictionary<ItemType, List<float>> items; //아이템들 카테고리<종류,수>
        public SaveDictionary<ItemType, List<WeaponArmorSaveData>> weaponArmor; //무기랑 갑옷<종류,수>

        //사전
        public List<int> getDictionaryPage;
        public List<string> getDictionaryPageMemo;
        
        //농사
        //public SaveDictionary<>
        
        [ContextMenu("ResetStat")]
        public void ResetStat()
        {
            sceneName = "JustTest";
            isChat = false;

            lastText = "마지막 대화가 없습니다.";
            
            ResetCharacter();

            year = 2000;
            month = 1;
            day = 1;
            hour = 1;
            minute = 0;
             
            ResetItem();

            getDictionaryPage = new List<int>();
            getDictionaryPageMemo = new List<string>();
        }
        
        public void ResetCharacter() //캐릭터들  전부 초기화
        {
            characterPositions = new SaveDictionary<EntityName, Vector2>();
            characterStats = new SaveDictionary<EntityName, SaveDictionary<StatsType, int>>();
            characterLastText = new SaveDictionary<EntityName, SaveDictionary<DialogType, string>>();
            characterLastText.Clear();

            foreach (EntityName name in Enum.GetValues(typeof(EntityName))) //이름들 저장
            {
                if((int)name >= 30000 || name == EntityName.None) continue;
                
                characterPositions.Add(name,Vector2.zero); //다 같은 자리라니... (주석)
                
                SaveDictionary<DialogType, string> di = new SaveDictionary<DialogType, string>();
                SaveDictionary<StatsType, int> st = new SaveDictionary<StatsType, int>();

                foreach (DialogType dialog in
                         Enum.GetValues(typeof(DialogType))) //모든 걸 저장 / 다이얼로그 종류 (챕터, 넘버, 텍스트, 메모, 신뢰도만 사용하긴 함.)
                {
                    di.Add(dialog, ""); // " " 초기화
                }

                characterLastText.Add(name, di); //저장

                foreach (StatsType stats in Enum.GetValues(typeof(StatsType))) //모든 걸 저장 / 다이얼로그 종류 (챕터, 넘버, 텍스트, 메모, 러브 만 사용하긴 함.)
                {
                    if(stats == StatsType.none || stats == StatsType.curHp) continue;
                    st.Add(stats,1);
                }
                st.Add(StatsType.curHp, 50);
                characterStats.Add(name, st);
            }
        }
        
        public void ResetItem() //스탯의 아이템 전부 초기화
        {
            items = new SaveDictionary<ItemType, List<float>>();
            weaponArmor = new SaveDictionary<ItemType, List<WeaponArmorSaveData>>();
            items.Clear();
            weaponArmor.Clear();

            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (type == ItemType.none)
                    continue;

                items.Add(type, new List<float>(){0}); //0으로 초기화
                if ((int)type / 1000 == (int)ItemCategory.weapon ||
                    (int)type / 1000 == (int)ItemCategory.armor) //무기와 갑옷
                {
                    weaponArmor.Add(type,new List<WeaponArmorSaveData>());
                }
            }
        }
    }
}
