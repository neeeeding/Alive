using System;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
using UnityEngine;
using Newtonsoft.Json;

namespace _02Script.SaveData
{
    public class GameSaveManager<T> : Singleton<T> where T : MonoBehaviour

    {
        //Action --------------------------------------------------------------------------
        public static Action OnStart; //모든 초기화 완료 후
        
        public readonly string GamePath = "gameSaveData"; // 저장 경로
        //변수들 --------------------------------------------------------------------------
        public static string GameSaveFilePath; //파일 위치
        
        [Header("Public")]
        public GameSaveData saveData; //기기에서만 저장 되는 것들 (ex: 저장 안한 진행사항)
        public PlayerStatSC PlayerStat; //플레이어 정보
        
        public bool isStart; //시작에 관해서

        [ContextMenu("ResetAll")]
        public virtual void ResetDate() //초기화 하기
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        protected virtual void Awake()
        {
            isStart = false;
            if (Instance != null && Instance != this) //더 있으면
            {
                Destroy(gameObject);
            }
            //TestGame(); //나중에 Load로 변경 할 거임 (주석)
            Load();
        }
        
        protected virtual void TestGame()
        {
            GameSaveData data = new GameSaveData();
            data.DataReset();
            data.stat.ResetStat();

            GameSaveFilePath = Application.persistentDataPath + "/Save";
            print(GameSaveFilePath);

            PlayerStat = data.stat; //로드
            
            saveData = new GameSaveData();
            saveData.stat = PlayerStat;
        }
        protected virtual void Load() //진짜 게임 용
        {
            //로드
            GameSaveData data;
            if (PlayerPrefs.GetString(GamePath) != "")
            {
                string json = PlayerPrefs.GetString(GamePath);
                if (string.IsNullOrEmpty(json)) return;

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                saveData = JsonConvert.DeserializeObject<GameSaveData>(json, settings);
    
                if (saveData?.stat != null)
                {
                    RestoreAllDictionaries(saveData.stat);
    
                    PlayerStat = saveData.stat;
                }
            }
            else //저장 된게 없으면 새 거
            {
                data = new GameSaveData();
                data.DataReset();
                data.stat.ResetStat();
                
                PlayerStat = data.stat; //로드
            
                saveData = new GameSaveData();
                saveData.stat = PlayerStat;
            }

            GameSaveFilePath = Application.persistentDataPath + "/Save";
            print(GameSaveFilePath);
        }
        
        private void RestoreAllDictionaries(PlayerStatSC s) //세이브 딕셔너리 때문에... (중첩)
        {
            s.items?.SyncDictFromList();
            s.characterPositions?.SyncDictFromList();
            s.items?.SyncDictFromList();
            s.weaponArmor?.SyncDictFromList();

            if (s.characterStats != null)
            {
                s.characterStats.SyncDictFromList();
                foreach (var innerDict in s.characterStats.vs)
                {
                    innerDict?.SyncDictFromList();
                }
            }

            if (s.characterLastText != null)
            {
                s.characterLastText.SyncDictFromList();
                foreach (var innerDict in s.characterLastText.vs)
                {
                    innerDict?.SyncDictFromList();
                }
            }
        }

        protected virtual void Start()
        {
            isStart = true;
            OnStart?.Invoke();
        }

        protected virtual void OnDestroy()
        {
            SaveData();
        }

        protected virtual void SaveData()
        {
            saveData.stat = PlayerStat;
            var settings = new JsonSerializerSettings 
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
            };

            string json = JsonConvert.SerializeObject(saveData, settings);
            PlayerPrefs.SetString(GamePath, json);
            PlayerPrefs.Save();
            print(json);
        }
    }
}

