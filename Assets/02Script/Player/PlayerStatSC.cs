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

        #region 이전 호감도 저장 법
        //[Space(50f)] //호감도
        //[Range(0, 100)]
        //public int resty;//레스티
        //[Range(0, 100)]
        //public int chris; //크리스
        //[Range(0, 100)]
        //public int theo; //테오
        //[Range(0, 100)]
        //public int pio; //피오
        //[Range(0, 100)]
        //public int noah; //노아
        //[Range(0, 100)]
        //public int nia; //니아
        //[Range(0, 100)]
        //public int villain; //빌런
        //[Range(0, 100)]
        //public int harry; //해리
        //[Range(0, 100)]
        //public int daniel; //다니엘
        #endregion

        #region 이전 내용 저장 법
        //public int[] restyLastText = new int[2]; // 레스티
        //public int[] chrisLastText = new int[2]; // 크리스
        //public int[] theoLastText = new int[2]; // 테오
        //public int[] pioLastText = new int[2]; // 피오
        //public int[] noahLastText = new int[2]; // 노아
        //public int[] niaLastText = new int[2]; // 니아
        //public int[] villainLastText = new int[2]; // 빌런
        //public int[] harryLastText = new int[2]; // 해리
        //public int[] danielLastText = new int[2]; // 다니엘
        #endregion

        [Space(50f)] //날짜
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;

        public SaveDictionary<ItemCategory, SaveDictionary<ItemType, int>> items; //아이템들 카테고리<종류,수>
    

        #region 이전 아이템 저장 법
        //public int lovePotionCount; //포션
        //public int staminaPotionCount;
        //public int painPotionCount;

        //public int blackbookCount; //책
        //public int healbookCount;
        //public int firebookCount;
        //public int waterbookCount;
        //public int copybookCount;
        //public int potionbookCount;

        //public int umbrellaCount; //우산
        //public int broomstickCount; //빗자루 (순간이동 아이템)
        //public int fanCount; //부채
        //public int hotPackCount; //핫팩
        //public int fryingPanCount; //후라이팬
        //public int flowerCount; //꽃다발 (연애)
        //public int foragingBinCount; //곤충 채집통

        //public int drugCount; //마약
        //public int boxCount; //상자

        //public int glassesCount; //속마음을 보는 안경 (올바른 선택지)
        //public int readingGlassesCount; //돋보기 (선호하는 아이템 2개)
        //public int emptyGlassCount; //빈 유리병

        //public int wormCount; //벌레 들
        //public int trashGlassCount; //실패한 유리병

        //public int perfectGlassCount; //생태계 유리병

        //public int restyMouse; //커서
        //public int chrisMouse; //커서
        //public int theoMouse; //커서
        //public int noahMouse; //커서
        //public int niaMouse; //커서
        //public int villainMouse; //커서
        //public int harryMouse; //커서
        //public int danielMouse; //커서
        //public int pioMouse; //커서

        //public int giftCount; //아직임
        #endregion

        [ContextMenu("ResetStat")]
        public void ResetStat()
        {
            blackMagic = 0;
            healMagic = 0;
            fireMagic = 0;
            waterMagic = 0;
            copyMagic = 0;
            potionMagic = 0;

            playerCoin = 5;

            job = PlayerJob.none;

            //resty = 0;
            //chris = 0;
            //theo = 0;
            //pio = 0;
            //noah = 0;
            //nia = 0;
            //villain = 0;
            //harry = 0;
            //daniel = 0;

            lastText = "마지막 대화가 없습니다.";

            year = 2000;
            month = 1;
            day = 1;
            hour = 1;
            minute = 0;
            demerit = 0;
        }
    }
}
