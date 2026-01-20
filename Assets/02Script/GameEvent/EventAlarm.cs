using System;
using System.Collections.Generic;
using _02Script.DoTweenUI.Warring;
using _02Script.Farming;
using _02Script.Obj.Obj;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace _02Script.GameEvent
{
    public class EventAlarm : Warring
    {
        [SerializeField] private float delay = 1;
        [SerializeField] private Transform parent;
        [SerializeField] private TeleportationBtn btnPrefabs;
        
        private Dictionary<EntityName, ObjTeleportationPos> _visit = new Dictionary<EntityName, ObjTeleportationPos>();
        private List<TeleportationBtn> _btns = new List<TeleportationBtn>();
        private Vector2 _btnSize;
        private RectTransform rectTransform;

        protected override void Awake()
        {
            _btnSize = btnPrefabs.GetComponent<RectTransform>().sizeDelta;
            base.Awake();
            // for (int i = 0; i < 4; i++)
            // {
            //     AddBtn(EntityName.isis);
            //     _btns[i].gameObject.SetActive(false);
            // }
        }

        public void TeleportationBtnsShowBtn()
        {
            //Vector3 targetPos = new Vector3(rectTransform.position.x, rectTransform.position.y + (_btnSize.y * _visit.Count), 0);
            Vector3 targetPos = new Vector3(rectTransform.position.x + _btnSize.x, rectTransform.position.y, 0);
            
            rectTransform.DOMove(targetPos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
        }
        public void TeleportationBtnsHideBtn()
        {
            Vector3 targetPos = new Vector3(rectTransform.position.x - _btnSize.x, rectTransform.position.y, 0);
            
            rectTransform.DOMove(targetPos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
        }

        public void Alarm([CanBeNull] Dictionary<EntityName, ObjTeleportationPos> characters)
        {
            if (characters.Count <= 0)
            {
                warringObj.SetActive(false);
                return;
            }
            _visit = characters;
            string massage = "";
            foreach (KeyValuePair<EntityName, ObjTeleportationPos> type in characters)
            {
                if (massage != "") massage += ", ";
                massage += ChatSetting.Name(type.Key);
            }

            massage += "이/가 방문했습니다.";
            
            base.ShowWarring(massage, 10);
        }

        public void AddBtnList(TeleportationBtn btn)
        {
            _btns.Add(btn);
            btn.gameObject.SetActive(false);
        }

        private void AddBtn(EntityName name)
        {
            TeleportationBtn btn;
            if (_btns.Count <= 0)
            {
                btn = Instantiate(btnPrefabs,parent);
                _btns.Add(btn);
            }
            btn = _btns[0];
            
            btn.gameObject.SetActive(true);
            btn.Setting(_visit[name], ChatSetting.Name(name), this);
            _btns.RemoveAt(0);
        }
    }
}