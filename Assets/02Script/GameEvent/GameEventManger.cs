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
        public static Action<DoUIType> OnLockUI; //봉인
        public static Action<TemperatureType> OnFarmTemperature; //온도
        
        [SerializeField] private SerializedDictionary<ObjTeleportationPos, CharacterEventData> characterEvent;
        [SerializeField] private EventAlarm eventAlarm;

        private int _curDay;
        private Dictionary<EntityName,ObjTeleportationPos> _nextDayDoEvent;

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
            _nextDayDoEvent = new Dictionary<EntityName, ObjTeleportationPos>();
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
                eventAlarm.Alarm(_nextDayDoEvent);
            }
        }

        private void EventCheck()
        {
            if(_curDay <= 0) return;
            foreach (KeyValuePair<ObjTeleportationPos, CharacterEventData> character in characterEvent)
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
                else character.Key.gameObject.SetActive(false);
            }
            
            if (_nextDayDoEvent.ContainsKey(EntityName.isis))
            {
                //전투는 당일날 이라서 (주석)
            }
        }

        private void DoCharacterEvent(ObjTeleportationPos character)
        {
            character.gameObject.SetActive(true);
            character.transform.position = characterEvent[character].pos.position;
            _nextDayDoEvent.Add(characterEvent[character].doEvent, character);
        }

        private void DoEvent()
        {
            if(_nextDayDoEvent.Count <= 0) return;
            
            if (_nextDayDoEvent.ContainsKey(EntityName.magenta))
            {
                //농사
                _temperature = Random.Range(0, 2) switch
                {
                    0 => TemperatureType.cold | TemperatureType.frigid,
                    _ => TemperatureType.highTemperature | TemperatureType.dry,
                };
            }
            if (_nextDayDoEvent.ContainsKey(EntityName.raelia))
            {
                //요리 & 농사 랜덤 봉인
                OnLockUI?.Invoke((DoUIType)(Random.Range((int)DoUIType.farm,(int)DoUIType.cook)));
            }
            
            _nextDayDoEvent = new Dictionary<EntityName, ObjTeleportationPos>();
        }

        private void LoadEvent()
        {
            _nextDayDoEvent = new Dictionary<EntityName, ObjTeleportationPos>();
            foreach (KeyValuePair<ObjTeleportationPos, CharacterEventData> character in characterEvent)
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
                _nextDayDoEvent.Add(characterEvent[character.Key].doEvent, character.Key);
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