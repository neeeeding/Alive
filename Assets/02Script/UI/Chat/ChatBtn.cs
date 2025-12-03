using System;
using _02Script.Manager;
using UnityEngine;

namespace _02Script.UI.Chat
{
    public class ChatBtn : MonoBehaviour
    {
        public static Action OnSkipChat;

        [Header("SaveBtn Need")] [SerializeField]
        private GameObject SaveWindow;

        [Space(0f)] [Header("Hide Need")] [SerializeField]
        private GameObject chatObj;

        [SerializeField] private GameObject[] inGame;

        private void Awake()
        {
            CloseSave();
        }

        public void LoadBtn() //로드 버튼 누를 때
        {
            UISettingManager.Instance.Save();
        }

        public void SaveBtn() //저장 버튼
        {
            SaveWindow.SetActive(true);
        }

        public void CloseSave() //저장 창 닫기
        {
            SaveWindow.SetActive(false);
        }

        public void SkipBtn() //스킵
        {
            OnSkipChat?.Invoke();
        }

        public void HideBtn() //비활성화 버튼
        {
            Show(false);
        }

        public void ShowBtn() //활성화
        {
            Show(true);
        }

        private void Show(bool show)
        {
            chatObj.SetActive(show);
            foreach (GameObject obj in inGame)
            {
                obj.SetActive(show);
            }
        }

        public void Likeability() //호감도 도감
        {
            UISettingManager.Instance.LikeabilityGuide();
        }

        public void LoveBtn() //선호 보기
        {

        }
    }
}
