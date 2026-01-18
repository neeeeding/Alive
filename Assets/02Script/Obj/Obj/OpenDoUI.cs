using System;
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

        #region EnDiAw

        private void OnEnable()
        {
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
            if (eventType == DoUIType.all &&
                doUiType != DoUIType.none)
            {
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