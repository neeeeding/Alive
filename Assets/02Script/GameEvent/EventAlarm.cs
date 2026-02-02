using System.Collections.Generic;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Obj.Entity;
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
        [SerializeField] private float delay = 0.2f;
        [SerializeField] private RectTransform parent;
        [SerializeField] private TeleportationBtn btnPrefabs;
        
        private Dictionary<EntityName, ObjTeleportationPos> _visit = new Dictionary<EntityName, ObjTeleportationPos>();
        private List<TeleportationBtn> _btns = new List<TeleportationBtn>();
        private Vector2 _btnSize;
        private RectTransform rectTransform;
        private bool _isShow;

        protected override void Awake()
        {
            _btnSize = btnPrefabs.GetComponent<RectTransform>().sizeDelta;
            base.Awake();
            _isShow = true;
            ClickBtn();
        }

        #region BtnActive
        public void ClickBtn()
        {
            if (_isShow)
                TeleportationBtnsHideBtn();
            else
                TeleportationBtnsShowBtn();
        }

        private void TeleportationBtnsShowBtn()
        {
            Vector3 targetPos = new Vector3(parent.sizeDelta.x, _btnSize.y * _visit.Count, 0);
            
            parent.DOSizeDelta(targetPos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
            _isShow = true;
        }
        private void TeleportationBtnsHideBtn()
        {
            Vector3 targetPos = new Vector3(parent.sizeDelta.x, 0, 0);
            
            parent.DOSizeDelta(targetPos, delay).SetEase(Ease.OutCirc).SetUpdate(true);
            _isShow = false;
        }
        #endregion

        public void Alarm([CanBeNull] Dictionary<EntityName, ObjTeleportationPos> characters)
        {
            parent.sizeDelta = new Vector3(parent.sizeDelta.x, 0, 0);
            
            warringObj.SetActive(false);
            if (characters.Count <= 0) return;
            
            _visit = characters;
            string massage = "";
            foreach (KeyValuePair<EntityName, ObjTeleportationPos> type in characters)
            {
                if (massage != "") massage += ", ";
                massage += EnumToString.Name(type.Key);
                
                AddBtn(type.Key);
            }

            massage += "가 방문했습니다.";
            
            ShowWarring(massage);
        }

        #region btn
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
            btn.Setting(_visit[name], EnumToString.Name(name), this);
            _btns.RemoveAt(0);
        }
        #endregion

        public override void ShowWarring(string massage = "오류가 발생했습니다.", float i = 1)
        {
            text.text = massage;
            warringObj.SetActive(true);
        }
    }
}