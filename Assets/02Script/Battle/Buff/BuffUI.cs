using System;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Buff
{
    public class BuffUI : MonoBehaviour
    {
        public static Action<BuffSO,float, Vector3, bool> OnMouseEnter; //정보, 현재 남은 시간 
        
        [SerializeField] protected Image buffImage;
        
        public BuffSO so;
        
        protected BuffManager manager;
        protected EntityName entity;
        protected float curTime;
        protected float curRepeatTime;
        protected float curRepeat; //반복한 수
        protected float buffDelay;
        protected bool isExplanation;
        protected bool isUI = false; //UI 인지

        #region Mouse
        public virtual void MouseEnter()
        {
            OnMouseEnter?.Invoke(so,-1, gameObject.transform.position,isUI);
            isExplanation = true;
        }
        public virtual void MouseExit()
        {
            OnMouseEnter?.Invoke(null,0,Vector3.zero,true);
            isExplanation = false;
        }
        #endregion

        #region EnDi
        protected virtual void OnEnable()
        {
            BuffManager.OnBuffDelay += BuffDelay;
        }
        protected virtual void OnDisable()
        {
            BuffManager.OnBuffDelay -= BuffDelay;
            if(isExplanation) MouseExit();
        }
        #endregion

        protected virtual void BuffDelay(StatsType type, float buffValue)
        {
            if (type == StatsType.tolerance && so.isDeBuff) //내성
            {
                buffDelay -= buffValue;
            }
            else if (type == StatsType.duration && !so.isDeBuff) //지속
            {
                buffDelay += buffValue;
            }
        }

        #region Set
        public virtual void BuffSet(BuffSO buff, BuffManager battleEntity, EntityName entity, bool isUI)
        {
            so = buff;
            manager = battleEntity;
            this.entity = entity;
            curTime = 0;
            curRepeatTime = 0;
            curRepeat = 1;
            buffDelay = so.buffDelay;
            buffImage.sprite = so.buffImage;
            this.isUI = isUI;
        }
        #endregion
    }
}