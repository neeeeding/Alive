using System;
using System.Collections.Generic;
using _02Script.Farming;
using _02Script.Manager;
using _02Script.Obj.Entity;
using _02Script.Obj.Obj;
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
        private Dictionary<EntityName,ObjTeleportationPos> _nextDayDoEvent = new Dictionary<EntityName, ObjTeleportationPos>();

        private TemperatureType _temperature;
        private bool isEnter;
        private bool _isNight;

        private void OnEnable()
        {
            _isNight = false;
            LoadCard.OnLoad += LoadEvent;
        }

        private void OnDisable()
        {
            LoadCard.OnLoad -= LoadEvent;
        }

        private void Start()
        {
            LoadEvent();
        }

        private void Update()
        {
            if (HouseManager.Instance.PlayerStat.hour >= 20 && !_isNight)
            {
                _isNight = true;
                OnLockUI?.Invoke(DoUIType.all);
            }
            if (_curDay != HouseManager.Instance.PlayerStat.day)
            {
                _curDay = HouseManager.Instance.PlayerStat.day;
                OnFarmTemperature?.Invoke(_temperature); //잠금 해제 시키려면 해야 함
                GetOutEveryone();
                isEnter = false;
            }
            if (HouseManager.Instance.PlayerStat.hour >= 8 && !isEnter)
            {
                _temperature = TemperatureType.warmth;
                OnFarmTemperature?.Invoke(_temperature);
                isEnter = true;
                EventCheck();
                eventAlarm.Alarm(_nextDayDoEvent);
                DoEvent(false);
            }
        }
        
        private void GetOutEveryone()
        {
            eventAlarm.Alarm(_nextDayDoEvent);
            
            foreach (KeyValuePair<ObjTeleportationPos, CharacterEventData> character in characterEvent)
            {
                character.Key.gameObject.SetActive(false);
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

            DoEvent(true);
        }

        private void DoCharacterEvent(ObjTeleportationPos character)
        {
            character.gameObject.SetActive(true);
            character.transform.position = characterEvent[character].pos.position;
            if(!_nextDayDoEvent.ContainsKey(characterEvent[character].doEvent))
                _nextDayDoEvent.Add(characterEvent[character].doEvent, character);
        }

        //당일과, 다음날 지속?
        private void DoEvent(bool isToday)
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
                OnFarmTemperature?.Invoke(_temperature);
            }
            if (_nextDayDoEvent.ContainsKey(EntityName.raelia))
            {
                //요리 & 농사 랜덤 봉인
                OnLockUI?.Invoke((DoUIType)(Random.Range((int)DoUIType.farm,(int)DoUIType.cook + 1)));
            }
            
            if(!isToday)
                _nextDayDoEvent = new Dictionary<EntityName, ObjTeleportationPos>();
        }

        private void LoadEvent()
        {
            _curDay = HouseManager.Instance.PlayerStat.day;
            _nextDayDoEvent = new Dictionary<EntityName, ObjTeleportationPos>();
            foreach (KeyValuePair<ObjTeleportationPos, CharacterEventData> character in characterEvent)
            {
                bool isEvent = false;
                foreach (int day in character.Value.day)
                {
                    if (_curDay-1 % Math.Abs(day) == 0 && _curDay > 1)
                    {
                        isEvent = true;
                        if (day < 0)
                        {
                            isEvent = false;
                            break;
                        }
                    }
                }
                if(isEvent) _nextDayDoEvent.Add(characterEvent[character.Key].doEvent, character.Key);
            }
            
            _temperature = TemperatureType.warmth;
            OnFarmTemperature?.Invoke(_temperature);
            EventCheck();
            eventAlarm.Alarm(_nextDayDoEvent);
            DoEvent(false);
        }
    }
    [Serializable]
    public struct CharacterEventData
    {
        public Transform pos;
        public EntityName doEvent;
        public int[] day;
    }
}