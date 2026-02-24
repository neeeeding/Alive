using System;
using System.Collections.Generic;
using _02Script.UI.person;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Buff
{
    public class Buff : MonoBehaviour
    {
        public static Action<BuffSO,float> OnMouseEnter; //정보, 현재 남은 시간 
        
        [SerializeField] private Image buffImage;
        
        public BuffSO so;
        public Dictionary<StatsType,float> buffValue; //버프 받는 값들
        
        private BuffManager _manager;
        private float _curTime;
        private float _curRepeatTime;
        private float _curRepeat; //반복한 수

        public void MouseEnter()
        {
            OnMouseEnter?.Invoke(so,_curTime);
        }

        public void MouseExit()
        {
            OnMouseEnter?.Invoke(null,0);
        }

        //체력 감소도 여기서 자동으로 해줄것.
        private void BuffTime()
        {
            _curTime += Time.deltaTime;
            _curRepeatTime += Time.deltaTime;

            if (_curRepeatTime >= so.repeatDelay && _curRepeat >= so.repeatDelay) //반복 값 증가
            {
                foreach (KeyValuePair<StatsType, float> value in so.useStatType)
                {
                    buffValue[value.Key] += value.Value;
                }

                _curRepeat++;
                _curRepeatTime = 0;
            }
            
            if(_curTime < so.buffDelay) return; //버프 종료
            _manager.EndBuff(this);
        }

        private void Update()
        {
            BuffTime();
        }

        public void BuffSet(BuffSO buff, BuffManager battleEntity)
        {
            so = buff;
            _manager = battleEntity;
            _curTime = 0;
            _curRepeatTime = 0;
            _curRepeat = 1;

            foreach (KeyValuePair<StatsType, float> value in so.useStatType)
            {
                buffValue.Add(value.Key, value.Value);
            }
        }
    }
}