using System;
using System.Collections.Generic;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Buff
{
    public class Buff : MonoBehaviour
    {
        public static Action<EntityName, float> OnDamage; // hp 감소
        public static Action<BuffSO,float> OnMouseEnter; //정보, 현재 남은 시간 
        
        [SerializeField] private Image buffImage;
        
        public BuffSO so;
        public Dictionary<StatsType,float> BuffValue = new Dictionary<StatsType, float>(); //버프 받는 값들
        
        private BuffManager _manager;
        private EntityName _entity;
        private float _curTime;
        private float _curRepeatTime;
        private float _curRepeat; //반복한 수
        private float _BuffDelay;

        #region Mouse
        public void MouseEnter()
        {
            OnMouseEnter?.Invoke(so,_curTime);
        }
        public void MouseExit()
        {
            OnMouseEnter?.Invoke(null,0);
        }
        #endregion

        #region EnDi
        private void OnEnable()
        {
            BuffManager.OnBuffDelay += BuffDelay;
        }
        private void OnDisable()
        {
            BuffManager.OnBuffDelay -= BuffDelay;
        }
        #endregion

        private void BuffDelay(StatsType type, float buffValue)
        {
            if (type == StatsType.tolerance && so.isDeBuff) //내성
            {
                _BuffDelay -= buffValue;
            }
            else if (type == StatsType.duration && !so.isDeBuff) //지속
            {
                _BuffDelay += buffValue;
            }
        }
        
        private void BuffTime()
        {
            _curTime += Time.deltaTime;
            _curRepeatTime += Time.deltaTime;

            if (_curRepeatTime >= so.repeatDelay && _curRepeat >= so.repeatDelay) //반복 값 증가
            {
                foreach (KeyValuePair<StatsType, float> value in so.useStatType)
                {
                    BuffValue[value.Key] += value.Value;
                    if (value.Key == StatsType.curHp) //체력 감소
                    {
                        OnDamage?.Invoke(_entity,value.Value);
                    }
                }

                _curRepeat++;
                _curRepeatTime = 0;
            }
            
            if(_curTime < _BuffDelay) return; //버프 종료
            _manager.EndBuff(this);
        }

        private void Update()
        {
            BuffTime();
        }

        #region Set
        public void BuffSet(BuffSO buff, BuffManager battleEntity, EntityName entity)
        {
            so = buff;
            _manager = battleEntity;
            _entity = entity;
            _curTime = 0;
            _curRepeatTime = 0;
            _curRepeat = 1;
            _BuffDelay = so.buffDelay;
            buffImage.sprite = so.buffImage;
            BuffValue.Clear();

            foreach (KeyValuePair<StatsType, float> value in so.useStatType)
            {
                BuffValue.Add(value.Key, value.Value);
            }
        }
        #endregion
    }
}