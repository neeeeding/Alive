using System;
using System.Collections.Generic;
using _02Script.Etc;
using _02Script.GoHouse.Block;
using _02Script.GoHouse.SO;
using _02Script.Inventory.Item;
using _02Script.SaveData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.GoHouse.Etc
{
    public class GoHouseSaveManager : GameSaveManager<GoHouseSaveManager>
    {
        public static Action<SaveDictionary<ItemType, List<float>>> OnSaveItem;
        
        private readonly string _battleItemSave = "battleGameSaveData"; // 저장 경로

        private SaveDictionary<ItemType, List<float>> _items
            = new SaveDictionary<ItemType, List<float>>(); //채집의 얻은 것들

        private int _loseItemCount;

        #region EnDi
        private void OnEnable()
        {
            LessSO.OnLess += Less;
            DieSO.OnDie += FailGame;
            BlockPlayer.OnReSet += FailGame;
            HouseSO.OnSuccess += Success;
        }
        private void OnDisable()
        {
            LessSO.OnLess -= Less;
            DieSO.OnDie -= FailGame;
            BlockPlayer.OnReSet -= FailGame;
            HouseSO.OnSuccess -= Success;
        }
        #endregion
        

        #region BlockAction
        private void Less()
        {
            PlayerStat = saveData.stat;
        }
        #endregion

        #region Success & Fail
        private void Success(string s,BlockActionSO SS)
        {
            SaveData();
        }
        protected override async void SaveData() //성공 || 스킵
        {
            //성공도 안했는데 끔으로 사기 치려는 사람을 방지 하려고
            PlayerPrefs.SetString(_battleItemSave,"");
            PlayerPrefs.Save();
            
            OnSaveItem?.Invoke(_items);
            await AsyncTime.WaitSeconds(1);
            base.SaveData();
        }

        private void FailGame() //실패시
        {
            List<ItemType> itemTypes = new List<ItemType>();

            foreach (KeyValuePair<ItemType, List<float>> item in _items.ToDictionary())
            {
                itemTypes.Add(item.Key);
                
                _loseItemCount = item.Value.Count;
            }

            _loseItemCount = (_loseItemCount / 100) * 5;

            for (int i = 0; i < _loseItemCount; i++)
            {
                int randomType = Random.Range(0, itemTypes.Count);

                _items[itemTypes[randomType]].
                    RemoveAt(Random.Range(0, _items[itemTypes[randomType]].Count));
                if (_items[itemTypes[randomType]].Count <= 0)
                {
                    _items.Remove(itemTypes[randomType]);
                    itemTypes.Remove(itemTypes[randomType]);
                }
            }
        }
        #endregion

        #region Save & Load
        protected override void Load()
        {
            base.Load();
            saveData.stat = PlayerStat;
            if (PlayerPrefs.GetString(_battleItemSave) != "")
            {
                string json = PlayerPrefs.GetString(_battleItemSave);
                _items = JsonUtility.FromJson<SaveDictionary<ItemType, List<float>>>(json);
            }
        }
        protected override void OnApplicationQuit()
        {
            //한 번 실패하고 스킵한 처리
            if (PlayerPrefs.GetString(_battleItemSave) != "")
            {
                FailGame();
                SaveData();
            }
        }
        #endregion
    }
}