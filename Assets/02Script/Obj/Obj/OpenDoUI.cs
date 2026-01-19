using System;
using _02Script.DoTweenUI.Warring;
using _02Script.GameEvent;
using UnityEngine;

namespace _02Script.Obj.Obj
{
    public class OpenDoUI : MonoBehaviour
    {
        [SerializeField] private DoUIType doUiType;
        [SerializeField] private GameObject doUi;
        private bool _isPlayer;
        private bool _isEvent;

        private string _addWarring;

        #region EnDiAw

        private void OnEnable()
        {
            _addWarring = "지금은 ";
            GameEventManger.OnLockUI += DontUi;
        }

        private void OnDisable()
        {
            GameEventManger.OnLockUI -= DontUi;
        }
        private void Awake()
        {
            _isPlayer = false;
            _isEvent = false;
            ClickObj();
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isPlayer = false;
            }
        }

        private void DontUi(DoUIType eventType)
        {
            _addWarring = "지금은 ";
            if (eventType == DoUIType.all &&
                doUiType != DoUIType.none)
            {
                _addWarring = "밤에는 ";
                _isEvent = true;
                return;
            }
            
            _isEvent = doUiType == eventType;
        }

        private void EndEvent()
        {
            _isEvent = false;
        }

        public void ClickObj()
        {
            doUi.SetActive(_isPlayer && !_isEvent);
            
            if(!_isEvent) return;
            WarringManager.Warring.ShowWarring(_addWarring +
                doUiType switch
                {
                    DoUIType.farm => "밭을 사용하실 수 없습니다.",
                    DoUIType.cook => "요리하실 수 없습니다.",
                    DoUIType.produce => "제작하실 수 없습니다.",
                    _=> "사용하실 수 없습니다.",
                });
        }
    }

    public enum DoUIType
    {
        none = 0,
        farm,
        cook,
        produce,
        all=99,
    }
}