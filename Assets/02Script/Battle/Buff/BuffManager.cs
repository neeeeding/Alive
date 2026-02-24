using System.Collections.Generic;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Battle.Buff
{
    public class BuffManager : MonoBehaviour
    {
        [SerializeField] private Buff buffPrefab;
        [SerializeField] private Transform buffPrent;
        
        private List<Buff> _buffs = new List<Buff>();
        private Dictionary<BuffType,List<Buff>> _typeBuffs = new Dictionary<BuffType, List<Buff>>(); //받는 버프 (중복 가능)
        
        public float BuffCalculate(StatsType stat) //버프 효과 계산
        {
            float buffValue = 0; //결과적으로 받는 버프의 값
            
            foreach (KeyValuePair<BuffType, List<Buff>> buff in _typeBuffs) //받고 있는 모든 버프
            {
                foreach (Buff card in buff.Value) //같은 버프들
                {
                    foreach (KeyValuePair<StatsType, float> value in card.buffValue) //해당 버프만
                    {
                        if (stat != value.Key) continue;
                        buffValue += value.Value;
                    }
                }
            }
            
            return buffValue;
        }

        public void EndBuff(Buff buff) //완료
        {
            buff.gameObject.SetActive(false);
            _typeBuffs[buff.so.buffType].Remove(buff);
            _buffs.Add(buff);
        }
        
        public void GetBuffs(BuffSO so) //버프를 얻음
        {
            if(_buffs.Count <= 0)
            {
                Buff buff = Instantiate(buffPrefab, buffPrent);
                buff.BuffSet(so,this);
                buff.gameObject.SetActive(false);
                _buffs.Add(buff);   
            }
            Buff newBuff = _buffs[0];
            
            //만들기
            if(!_typeBuffs.ContainsKey(so.buffType))
            {
                _typeBuffs.Add(so.buffType,new List<Buff>());
            }
            
            if (so.isOverlap) //중복가능
            {
                _typeBuffs[so.buffType].Add(newBuff);
                newBuff.BuffSet(so,this);
            }
            else //중복 아닐 시 갱신
            {
                newBuff = _typeBuffs[so.buffType][0];
                newBuff.BuffSet(so,this);
            }
            newBuff.gameObject.SetActive(true);
        }
    }
}