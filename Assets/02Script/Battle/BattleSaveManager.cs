using System.Collections.Generic;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.SaveData;
using UnityEngine;

namespace _02Script.Battle
{
    public class BattleSaveManager : GameSaveManager
    {
        private readonly string _battleItemSave = "battleGameSaveData"; // 저장 경로

        private SaveDictionary<ItemType, List<float>> _items
            = new SaveDictionary<ItemType, List<float>>(); //채집의 얻은 것들

        private void GetItem(SaveDictionary<ItemType, List<float>> inventory)
        {
            foreach (KeyValuePair<ItemType, List<float>> item in inventory.ToDictionary())
            {
                if (!_items.ToDictionary().ContainsKey(item.Key))
                {
                    _items.Add(item.Key, item.Value);
                }
                else
                {
                    foreach (float count in item.Value)
                    {
                        _items[item.Key].Add(count);
                    }
                }
            }
        }
        
        protected override void SaveData() //성공 할 때만
        {
            base.SaveData();
            
            string json = JsonUtility.ToJson(_items);
            PlayerPrefs.SetString(_battleItemSave, json);
            PlayerPrefs.Save();
        }

        private void FailGame() //실패시 비워버림
        {
            PlayerPrefs.SetString(GamePath,"");
            PlayerPrefs.Save();
        }
        protected override void OnApplicationQuit()
        {
        }
    }
}