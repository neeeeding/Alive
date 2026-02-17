using System;
using _02Script.Etc;
using _02Script.Manager;
using UnityEngine;

namespace _02Script.SaveData
{
    public class GameSaveManager: Singleton<HouseManager>
    {
        //Action --------------------------------------------------------------------------
        public static Action OnStart; //모든 초기화 완료 후
        
        public readonly string GamePath = "gameSaveData"; // 저장 경로
        //변수들 --------------------------------------------------------------------------
        public static string GameSaveFilePath; //파일 위치
        
        [Header("Public")]
        public GameSaveData saveData; //기기에서만 저장 되는 것들 (ex: 저장 안한 진행사항)
        public PlayerStatSC PlayerStat; //플레이어 정보

        [ContextMenu("ResetAll")]
        public virtual void ResetDate() //초기화 하기
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this) //더 있으면
            {
                Destroy(gameObject);
            }
            TestGame(); //나중에 Load로 변경 할 거임 (주석)
        }
        
        protected virtual void TestGame()
        {
            GameSaveData data = new GameSaveData();
            data.DataReset();
            data.stat.ResetStat();

            GameSaveFilePath = Application.persistentDataPath + "/Save";
            print(GameSaveFilePath);

            PlayerStat = data.stat; //로드
            
            saveData.stat = PlayerStat;
        }
        protected virtual void Load() //진짜 게임 용
        {
            //로드
            GameSaveData data;
            if (PlayerPrefs.GetString(GamePath) != "")
            {
                string json = PlayerPrefs.GetString(GamePath);
                data = JsonUtility.FromJson<GameSaveData>(json);
                saveData = data;
            }
            else //저장 된게 없으면 새 거
            {
                data = new GameSaveData();
                data.DataReset();
                data.stat.ResetStat();
            }

            GameSaveFilePath = Application.persistentDataPath + "/Save";
            print(GameSaveFilePath);

            PlayerStat = data.stat; //로드
        }

        protected virtual void Start()
        {
            OnStart?.Invoke();
        }

        protected virtual void OnApplicationQuit()
        {
            SaveData();
        }

        protected virtual void SaveData()
        {
            //정보 저장
            saveData.stat = PlayerStat;
            
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(GamePath, json);
            PlayerPrefs.Save();
        }
    }
}

