using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Player;
using _02Script.UI.Likeability;
using _02Script.Etc;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Dialog.Entity;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Manager
{
    public class UISettingManager : Singleton<UISettingManager>
    {
        [Header("Need")]
        [SerializeField] private GameObject[] inGame; //인 게임 필요 요소 (ex : 돈, 설정...)
        [SerializeField] private Dialog chat; //채팅
        [SerializeField] private GameObject coin; //코인 상점

        [SerializeField] private GameObject backBtn; //인 게임으로

        [Space(10f)]
        [Header("Need(setting)")]
        [SerializeField] private GameObject allSetting; //설정들 다 모아둔 어미

        [SerializeField]private SerializedDictionary<UIActiveType, GameObject> uiObj = new SerializedDictionary<UIActiveType, GameObject>();
        
        [SerializeField] private LikeItemManager likeItemManager; //선호 아이템 매니저

        [Space(10)]
        [Header("Select")]
        [SerializeField] private GameObject obj; //상점(혹은 교시 고르기)

        private Dictionary<UIActiveType, bool> uiActiveBool = new Dictionary<UIActiveType, bool>();

        #region EnDiAw
        private void Awake()
        {
            DicSetting();
        }
        private void OnEnable()
        {
            DialogEntity.OnChat += Chat;
        }
        private void OnDisable()
        {
            DialogEntity.OnChat -= Chat;
        }
        #endregion

        public void InGame() //게임으로
        {
            GameManager.Instance.PlayerStat.isChat = false;
            AllHide();
            SettingAll();
            Time.timeScale = 1f;
            PlayerInput.Instance.CanInput();
        }

        public void CloseChat() //채팅 닫기
        {
            SetBool(UIActiveType.chat, false);
            GameManager.Instance.PlayerStat.isChat = false;
        }

        public void Coin() //코인 상점
        {
            SetBool(UIActiveType.coin,true);
        }

        public void Obj() //상점 혹은 학교 수업 선택
        {
            SetBool(UIActiveType.obj, true);
        }

        public void Chat(DialogEntitySO so, DialogEntity dialogEntity) //채팅
        {
            SetBool(UIActiveType.chat,true);
            chat.DialogSetting(so, dialogEntity);
            GameManager.Instance.PlayerStat.isChat = true;
        }

        public void Profile() //프로필
        {
            SetBool(UIActiveType.profile,true);
        }

        public void LiKeItem(DialogEntitySO so) //선호 아이템
        {
            SetBool(UIActiveType.likeItem, true);
            SetBool(UIActiveType.likeabilityGuide, true);
            likeItemManager.Setting(so);
        }

        public void LikeabilityGuide() //호감도
        {
            SetBool(UIActiveType.likeabilityGuide, true);
        }

        public void Map() //지도
        {
            SetBool(UIActiveType.map, true);
        }

        public void Setting() // 세팅
        {
            SetBool(UIActiveType.setting, true);
        }

        public void Save() //저장
        {
            SetBool(UIActiveType.save, true);
        }

        private void SettingAll() //세팅들
        {
            foreach (UIActiveType obj in uiObj.Keys)
            {
                uiObj[obj].SetActive(uiActiveBool[obj]);
            }
            
            foreach (GameObject obj in inGame)
            {
                obj.SetActive(!uiActiveBool[UIActiveType.chat]);
            }

            bool all = false;
            
            foreach (UIActiveType key in uiActiveBool.Keys)
            {
                if(key == UIActiveType.chat || key == UIActiveType.obj
                   || key == UIActiveType.coin) continue;

                if (uiActiveBool[key])
                {
                    all = true;
                    break;
                }
            }
            
            allSetting.SetActive(all);
            backBtn.SetActive(all || uiActiveBool[UIActiveType.coin] || uiActiveBool[UIActiveType.obj] );

            Time.timeScale = 0f;
        }

        private void AllHide() //전부 숨기기
        {
            foreach (UIActiveType boolKey in uiActiveBool.Keys.ToList())
            {
                if (boolKey == UIActiveType.chat) continue;
                uiActiveBool[boolKey] = false;
            }
        }

        private void SetBool(UIActiveType type, bool value)
        {
            AllHide();
            uiActiveBool[type] = value;
            if (value)
            {
                SettingAll();
                return;
            }
            
            SettingAll();
            foreach (KeyValuePair<UIActiveType, bool> one in uiActiveBool)
            {
                if(one.Value) return;
            }
            InGame();
        }

        private void DicSetting()
        {
            foreach(UIActiveType name in Enum.GetValues(typeof(UIActiveType)))
            {
                uiActiveBool.Add(name, false);
            }
            InGame();
        }
    }
    
    public enum UIActiveType
    {
        chat = 0, // !isInGame
        obj,
        coin,
        profile,
        likeItem,
        likeabilityGuide,
        map,
        setting,
        save,
    }
}
