using System;
using System.Collections.Generic;
using _02Script.Farming;
using _02Script.Manager;
using _02Script.Obj.Obj;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.Save;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.GameEvent
{
    public class GameEventManger : MonoBehaviour
    {
        public static Action<DoUIType> OnLockUI;
        public static Action<TemperatureType> OnFarmTemperature;
        
        [SerializeField] private SerializedDictionary<GameObject, CharacterEventData> characterEvent;

        private int _curDay;
        private List<EntityName> _nextDayDoEvent;

        private TemperatureType _temperature;

        private void OnEnable()
        {
            LoadEvent();
            LoadCard.OnLoad += LoadEvent;
        }

        private void OnDisable()
        {
            LoadCard.OnLoad -= LoadEvent;
        }

        private void Awake()
        {
            _nextDayDoEvent = new List<EntityName>();
            _curDay = 0;
        }

        private void Update()
        {
            if (GameManager.Instance.PlayerStat.hour >= 20)
            {
                OnLockUI?.Invoke(DoUIType.all);
            }
            if (_curDay != GameManager.Instance.PlayerStat.day)
            {
                _temperature = TemperatureType.warmth;
                DoEvent();
                _curDay = GameManager.Instance.PlayerStat.day;
                EventCheck();
                OnFarmTemperature?.Invoke(_temperature);
            }
        }

        private void EventCheck()
        {
            if(_curDay <= 0) return;
            foreach (KeyValuePair<GameObject, CharacterEventData> character in characterEvent)
            {
                bool isShow = false;
                foreach (int day in character.Value.day)
                {
                    if (_curDay % Math.Abs(day) == 0)
                    {
                        isShow = day >= 0;
                        if(day < 0) break;
                    }
                }
                if(isShow) DoCharacterEvent(character.Key);
                else character.Key.SetActive(false);
            }
            
            if (_nextDayDoEvent.Contains(EntityName.isis))
            {
                //전투는 당일날 이라서 (주석)
            }
        }

        private void DoCharacterEvent(GameObject character)
        {
            character.SetActive(true);
            character.transform.position = characterEvent[character].pos.position;
            _nextDayDoEvent.Add(characterEvent[character].doEvent);
        }

        private void DoEvent()
        {
            if(_nextDayDoEvent.Count <= 0) return;
            
            if (_nextDayDoEvent.Contains(EntityName.magenta))
            {
                //농사
                _temperature = Random.Range(0, 2) switch
                {
                    0 => TemperatureType.cold | TemperatureType.frigid,
                    _ => TemperatureType.highTemperature | TemperatureType.dry,
                };
            }
            if (_nextDayDoEvent.Contains(EntityName.raelia))
            {
                //요리 & 농사 랜덤 봉인
                OnLockUI?.Invoke((DoUIType)(Random.Range((int)DoUIType.farm,(int)DoUIType.cook)));
            }
            
            _nextDayDoEvent = new List<EntityName>();
        }

        private void LoadEvent()
        {
            foreach (KeyValuePair<GameObject, CharacterEventData> character in characterEvent)
            {
                bool isEvent = false;
                foreach (int day in character.Value.day)
                {
                    if (_curDay-1 % Math.Abs(day) == 0)
                    {
                        isEvent = day >= 0;
                        if(day < 0) break;
                    }
                }
                _nextDayDoEvent.Add(characterEvent[character.Key].doEvent);
            }
            
            DoEvent();
            EventCheck();
        }
    }
    [System.Serializable]
    public struct CharacterEventData
    {
        public Transform pos;
        public EntityName doEvent;
        public int[] day;
    }
}