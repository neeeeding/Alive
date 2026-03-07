using System;
using System.Collections.Generic;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Buff
{
    public class Buff : BuffUI
    {
        public static Action<EntityName, float> OnDamage; // hp 감소
        
        public Dictionary<StatsType,float> BuffValue = new Dictionary<StatsType, float>(); //버프 받는 값들

        public override void MouseEnter()
        {
            OnMouseEnter?.Invoke(so,curTime, gameObject.transform.position,isUI);
            isExplanation = true;
        }

        private void BuffTime()
        {
            curTime += Time.deltaTime;
            curRepeatTime += Time.deltaTime;

            if (curRepeatTime >= so.repeatDelay && curRepeat >= so.repeatDelay) //반복 값 증가
            {
                foreach (KeyValuePair<StatsType, float> value in so.useStatType)
                {
                    BuffValue[value.Key] += value.Value;
                    if (value.Key == StatsType.curHp) //체력 감소
                    {
                        OnDamage?.Invoke(entity,value.Value);
                    }
                }

                curRepeat++;
                curRepeatTime = 0;
            }
            
            if(curTime < buffDelay) return; //버프 종료
            manager.EndBuff(this);
        }

        private void Update()
        {
            BuffTime();
        }

        #region Set
        public override void BuffSet(BuffSO buff, BuffManager battleEntity, EntityName entity, bool isUI)
        {
            base.BuffSet(buff, battleEntity, entity, isUI);
            
            BuffValue.Clear();

            foreach (KeyValuePair<StatsType, float> value in so.useStatType)
            {
                BuffValue.Add(value.Key, value.Value);
            }
        }
        #endregion
    }
}