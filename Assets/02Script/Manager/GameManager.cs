using System;
using System.Collections;
using _02Script.Etc;
using _02Script.Item;
using _02Script.UI.Dialog.Entity;
using _02Script.Player;
using _02Script.UI.Dialog;
using _02Script.UI.Dialog.Dialog;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public static string GameSaveFilePath; //파일 위치
        public string GamePath = "gameSaveData"; // 저장 경로

        public static Action OnNextDay; //다음날이 됨.
        public static Action CoinText; //코인 수 갱신 (텍스트)
        public static Action OnStart; //모든 초기화 완료 후

        public string curScene; // 현재 씬 이름

        [Header("Setting")] [SerializeField] private float dayTimeDelay = 10f;
        public GameSaveData saveData; //기기에서만 저장 되는 것들 (ex: 저장 안한 진행사항)
        public PlayerStatSC PlayerStat; //플레이어 정보
        public Player.Player Player; //플레이어 (state 조정 해줌(?))
        [Space(20f)] public ItemSO Item; //들고 있는 아이템?
        public ItemHold itemPos; //플레이어가 들고 있을 아이템 위치
        [Space(10f)] public bool isStart;

        [ContextMenu("ResetAll")]
        public void ResetDate() //초기화 하기
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) //더 있으면
            {
                Destroy(gameObject);
            }

            //로드
            GameSaveData data;
            // if (PlayerPrefs.GetString(GamePath) != "")
            // {
            //     string jsson = PlayerPrefs.GetString(GamePath);
            //     data = JsonUtility.FromJson<GameSaveData>(jsson);
            //     saveData = data;
            // }
            // else //저장 된게 없으면 새 거
            {
                data = new GameSaveData();
                data.DataReset();
                data.stat.ResetStat();
            }

            GameSaveFilePath = Application.persistentDataPath + "/Save";
            print(GameSaveFilePath);
            isStart = false;

            PlayerStat = data.stat; //로드

            //Player = gameObject.GetComponent<Player>();
            
            ItemCard.OnHoldItem += hold => Item = hold;

            DontDestroyOnLoad(gameObject); //삭제 되지 말라고


            //SceneManager.LoadScene(PlayerStat.sceneName);

            curScene = SceneManager.GetActiveScene().name; //현재 씬 알려주기

            StartCoroutine(nowDate());
        }

        private void Start()
        {
            OnStart?.Invoke();
            isStart = true;
        }

        private void OnApplicationQuit()
        {
            //정보 저장
            saveData.stat = PlayerStat;


            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(GamePath, json);
            PlayerPrefs.Save();
        }

        public void SetLove(DialogEntitySO dialogEntity, int love) //정보 넣고 해당 호감도 스탯에서의 이름 찾아서 전해주기
        {
            int.TryParse(PlayerStat.characterLastText[dialogEntity.EntityName][DialogType.Love],
                out int basic); //원래 값 가져오기

            PlayerStat.characterLastText[dialogEntity.EntityName][DialogType.Love] =
                (basic + love > 100 ? 100 : basic + love).ToString(); //저장해주기 (100초과시 걍 100)
        }

        public void AddCoin(int num) //코인 수
        {
            PlayerStat.playerCoin += num;
            CoinText?.Invoke();
        }

        public void AddItemCount(ItemCategory category, ItemType type, int num) //얻은, 잃은 아이템 수들
        {
            PlayerStat.items[category][type] += num;
        }

        private void ResetValue() //값 세팅
        {
            PlayerStat.ResetStat();
        }

        private IEnumerator nowDate() //시간세는거
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

