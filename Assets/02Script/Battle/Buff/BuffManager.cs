using System;
using System.Collections.Generic;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using UnityEngine;

namespace _02Script.Battle.Buff
{
    public class BuffManager : MonoBehaviour
    {
        public static Action<StatsType, float> OnBuffDelay; //버프 지속 시간 (음수 / 양수)
        
        [SerializeField] private Buff buffPrefab;
        [SerializeField] private Transform buffPrent;
        [SerializeField] private bool isUI = false; //UI 인지
        
        private EntityName _entity;
        private List<Buff> _buffs = new List<Buff>();
        private Dictionary<BuffType,List<Buff>> _typeBuffs = new Dictionary<BuffType, List<Buff>>(); //받는 버프 (중복 가능)
        
        public float BuffCalculate(StatsType stat) //버프 효과 계산
        {
            float buffValue = 0; //결과적으로 받는 버프의 값
            
            foreach (KeyValuePair<BuffType, List<Buff>> buff in _typeBuffs) //받고 있는 모든 버프
            {
                foreach (Buff card in buff.Value) //같은 버프들
                {
                    foreach (KeyValuePair<StatsType, float> value in card.BuffValue) //해당 버프만
                    {
                        if (stat != value.Key) continue;
                        buffValue += value.Value;
                    }
                }
            }
            
            return buffValue;
        }

        //내성 혹은 지속일시 액션 보내서 버프 딜레이 조정하기
        public void EndBuff(Buff buff) //버프 종료
        {
            buff.gameObject.SetActive(false);
            _typeBuffs[buff.so.buffType].Remove(buff);
            _buffs.Add(buff);

            if (buff.so.useStatType.ContainsKey(StatsType.tolerance)) //내성
            {
                OnBuffDelay?.Invoke(StatsType.tolerance,-buff.so.useStatType[StatsType.tolerance]);
            }
            else if (buff.so.useStatType.ContainsKey(StatsType.duration)) //지속
            {
                OnBuffDelay?.Invoke(StatsType.duration,-buff.so.useStatType[StatsType.duration]);
            }
        }
        
        //내성 혹은 지속일시 액션 보내서 버프 딜레이 조정하기
        public void GetBuffs(BuffSO so) //버프를 얻음
        {
            if(so == null) return;
            
            if(_buffs.Count <= 0)
            {
                Buff buff = Instantiate(buffPrefab, buffPrent);
                buff.gameObject.SetActive(false);
                _buffs.Add(buff);   
            }
            Buff newBuff = _buffs[0];
            
            //만들기
            if(!_typeBuffs.ContainsKey(so.buffType))
            {
                _typeBuffs.Add(so.buffType,new List<Buff>());
            }
            
            if (so.isOverlap || _typeBuffs[so.buffType].Count <= 0) //중복가능
            {
                _typeBuffs[so.buffType].Add(newBuff);
                newBuff.BuffSet(so,this,_entity,isUI);
            }
            else if(!so.isOverlap)//중복 아닐 시 갱신
            {
                newBuff = _typeBuffs[so.buffType][0];
                newBuff.BuffSet(so,this,_entity,isUI);
            }
            newBuff.gameObject.SetActive(true);
            
            if (so.useStatType.ContainsKey(StatsType.tolerance)) //내성
            {
                OnBuffDelay?.Invoke(StatsType.tolerance,so.useStatType[StatsType.tolerance]);
            }
            else if (so.useStatType.ContainsKey(StatsType.duration)) //지속
            {
                OnBuffDelay?.Invoke(StatsType.duration,so.useStatType[StatsType.duration]);
            }
        }

        public void SetEntity(EntityName entity)
        {
            _entity = entity;
        }
    }
}