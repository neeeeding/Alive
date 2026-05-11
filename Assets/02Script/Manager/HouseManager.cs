using System;
using System.Collections;
using _02Script.GamePlayer.GamePlayer;
using _02Script.Inventory.Item;
using _02Script.UI.Dialog.Entity;
using _02Script.SaveData;
using _02Script.UI.Dialog.Dialog;
using UnityEngine;

namespace _02Script.Manager
{
    public class HouseManager : GameSaveManager<HouseManager>
    {
        //Action --------------------------------------------------------------------------
        public static Action OnNextDay; //다음날이 됨.
        public static Action CoinText; //코인 수 갱신 (텍스트)
        
        //readonly ------------------------------------------------------------------------
        public readonly int WalkSpeed = 5;
        public readonly int RunSpeed = 5 * 2;
        
        //변수들 --------------------------------------------------------------------------
        [Header("Setting")]
        [Tooltip("base : 0.5 / test : 1/60/5 == (0.003333333)")]
        [SerializeField] private float dayTimeDelay = 10f;
        
        [Header("Public")]
        public Player housePlayer; //플레이어 (state 조정 해줌(?))
        public ItemHold itemPos; //플레이어가 들고 있을 아이템 위치
        public ItemDataSO holdItemData;

        [ContextMenu("ResetAll")]
        public override void ResetDate() //초기화 하기
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        protected override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad(gameObject); //삭제 되지 말라고
            
            StartCoroutine(NowDate());
            Time.timeScale = 1;
            if (CompareMonth(PlayerStat.day, PlayerStat.month))
            {
                PlayerStat.day = 1;
                PlayerStat.month++;

                if (PlayerStat.month > 12)
                {
                    PlayerStat.year++;
                }
            }
        }

        public void SetLove(DialogEntitySO dialogEntity, int love) //정보 넣고 해당 신뢰도 스탯에서의 이름 찾아서 전해주기
        {
            int.TryParse(PlayerStat.characterLastText[dialogEntity.EntityName][DialogType.Love],
                out int basic); //원래 값 가져오기

            PlayerStat.characterLastText[dialogEntity.EntityName][DialogType.Love] =
                (basic + love > 100 ? 100 : basic + love).ToString(); //저장해주기 (100초과시 걍 100)
        }

        /**얻은, 잃은 아이템 수들/
         * 일반 : 개수/
         * 음식 : 등급/
         * 소모품 : 내구도/
         * 잃 : 음수, 얻 : 양수/
         */
        public void AddItemCount(ItemType type, int num)
        {
            ItemCategory category = (ItemCategory)((int)type / 1000);
            switch(category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    PlayerStat.items[type][0] += num;
                    break;
                
                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    if (num < 0)
                        PlayerStat.items[type].Remove(num);
                    else
                        PlayerStat.items[type].Add(num);
                    break;
            }
        }

        private IEnumerator NowDate() //시간세는거
        {
            while (true)
            {
                yield return new WaitForSeconds(dayTimeDelay);

                PlayerStat.minute++;
                if (PlayerStat.minute >= 60)
                {
                    PlayerStat.minute = 0;
                    PlayerStat.hour++;

                    if (PlayerStat.hour >= 24)
                    {
                        PlayerStat.hour = 1;
                        PlayerStat.day++;
                        OnNextDay?.Invoke();

                        if (CompareMonth(PlayerStat.day, PlayerStat.month))
                        {
                            PlayerStat.day = 1;
                            PlayerStat.month++;

                            if (PlayerStat.month > 12)
                            {
                                PlayerStat.year++;
                            }
                        }

                    }
                }
            }
        }

        private bool CompareMonth(int day, int month) //월 계산
        {
            if (day < 28) return false;

            int[] day31 = { 1, 3, 5, 7, 8, 10, 12 };
            int[] day30 = { 4, 6, 9, 11 };

            if ((day > 28 && month == 2) || (day > 30 && Array.Exists(day30, x => x == month)) ||
                (day > 31 && Array.Exists(day31, x => x == month)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}

