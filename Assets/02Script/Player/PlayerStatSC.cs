using System;
using _02Script.Item;
using _02Script.Obj.Character;
using _02Script.UI.Chat;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Player
{
    [Serializable]
    public class PlayerStatSC
    {
        public string sceneName; //활동 하던 씬이름

        public bool isChat; //ture : 채팅 중, false : 인 게임

        [Range(0,100)] //능력치
        public float blackMagic;
        [Range(0, 100)]
        public float healMagic;
        [Range(0, 100)]
        public float fireMagic;
        [Range(0, 100)]
        public float waterMagic;
        [Range(0, 100)]
        public float copyMagic;
        [Range(0, 100)]
        public float potionMagic;
        [Range(0, 20)] //벌점
        public float demerit;

        public int playerCoin; //소지금

        public Vector2 playerPosition; //플레이어 위치

        public PlayerJob job; //전공

        [Space(50f)]
        public Character lastCharacter; //마지막 캐릭터
        public CharacterSO lastSO;
        public string lastText; //마지막 대화

        public SaveDictionary<CharacterName, SaveDictionary<DialogType, string>> characterlastText; //캐릭터 마지막 대화 이름<다이얼로그(종류), 번째(혹은 텍스트)>

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
            
            //능력치
            blackMagic = 0;
            healMagic = 0;
            fireMagic = 0;
            waterMagic = 0;
            copyMagic = 0;
            potionMagic = 0;
            demerit = 0;

            playerCoin = 5;
            playerPosition = new Vector2(0, 0);

            job = PlayerJob.none;

            lastText = "마지막 대화가 없습니다.";

            year = 2000;
            month = 1;
            day = 1;
            hour = 1;
            minute = 0;
        }
    }
}
