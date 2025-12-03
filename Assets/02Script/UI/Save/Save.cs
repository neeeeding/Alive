using UnityEngine;
using TMPro;
using System.IO;
using _02Script.Manager;
using _02Script.Player;

namespace _02Script.UI.Save
{
    public class Save : MonoBehaviour
    {
        [Header("All Need")] [SerializeField] private GameObject deleteWindow; //삭제 확인 창
        [SerializeField] private GameObject oneLoadCard; //세이브 파일 하나
        [SerializeField] private GameObject content; //보관

        [Space(20f)] [Header("Save_save Need")] [SerializeField]
        private TMP_InputField saveInputName; //기본 저장 파일명

        [SerializeField] private GameObject saveNameMessage; //파일이름 작성 메시지

        [Space(20f)] [Header("Chat_save Need")] [SerializeField]
        private TMP_InputField chatInputName; //채팅 저장 파일명

        [SerializeField] private GameObject chatNameMessage; //파일이름 작성 메시지

        private string path; //파일 저장 위치
        private string[] fileNames; //저장 파일 이름들
        private LoadCard deleteCard; //삭제할 저장 파일 (임시)

        private void Awake()
        {
            SettingPath();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); // 폴더 생성
            }

            if (File.Exists($"{path}/saveName"))
            {
                fileNames = File.ReadAllLines($"{path}/saveName");
                LoadSave();
            }

            CloseMessage();
        }

        private void OnEnable()
        {
            LoadCard.OnDelete += DeleteLoad;
        }

        public void DeleteBtn() //삭제 (예)
        {
            deleteCard.DeleteMe();
            CloseMessage();
        }

        public void SaveBtn() //저장 버튼 누를 때 (이름 작성 창 뜨기)
        {
            saveNameMessage.SetActive(true);
        }

        public void CompleteBtn() //(이름 작성) 완료 버튼
        {
            TMP_InputField inputName = chatInputName;
            if (saveInputName.text != "")
            {
                inputName = saveInputName;
            }

            if (inputName.text != "")
            {
                GameObject loadCard = Instantiate(oneLoadCard, content.transform);
                loadCard.GetComponent<LoadCard>().ClcikSave(inputName.text);
                SettingPath(); //혹시 모르니 재 세팅
                File.AppendAllText($"{path}/SaveName", $"{inputName.text}\n");

                CloseMessage();
                inputName.text = "";
            } //파일명 겹치는거 고려
        }

        public void CloseMessage() //모든 창 (이름작성, 삭제 확인) 닫기
        {
            saveNameMessage.SetActive(false);
            chatNameMessage.SetActive(false);
            deleteCard = null;
            deleteWindow.SetActive(false);
        }

        private void LoadSave() //저장하기
        {
            for (int i = 0; i < fileNames.Length; i++)
            {
                GameObject loadCard = Instantiate(oneLoadCard, content.transform);

                string data = File.ReadAllText($"{path}/{fileNames[i]}");
                PlayerStatSC stat = JsonUtility.FromJson<PlayerStatSC>(data);

                loadCard.GetComponent<LoadCard>().AwakeLoadSave(fileNames[i], stat);
            }
        }

        private void DeleteLoad(LoadCard card) //카드 삭제하기 창
        {
            deleteWindow.SetActive(true);
            deleteCard = card;

        }

        private void SettingPath()
        {
            if (GameManager.GameSaveFilePath != null && GameManager.GameSaveFilePath != "" && path != "")
            {
                path = GameManager.GameSaveFilePath;
            }
            else
            {
                path = Application.persistentDataPath + "/Save";
            }
        }

        private void OnDisable()
        {
            LoadCard.OnDelete -= DeleteLoad;
        }
    }
}
