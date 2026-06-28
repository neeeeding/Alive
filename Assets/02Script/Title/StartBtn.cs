using _02Script.Manager;
using _02Script.SaveData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Title
{
    public class StartBtn : MonoBehaviour
    {
        [SerializeField] private GameObject goStartList;
        [SerializeField] private string startScene = "AM_House";
        [SerializeField] private string tutorialScene = "Tutorial_House";

        public void NewGame()
        {
            GameSaveData data = new GameSaveData();
            data.DataReset();
            data.stat.ResetStat();
                
            // [수정] 세이브 초기화 및 중복 로드 제거
            PlayerPrefs.SetString("gameSaveData", "");
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(tutorialScene);
        }
        
        public void StartGame()
        {
            // [수정] 중복 LoadScene 호출 제거
            SceneManager.LoadScene(startScene);
        }

        public void GoStartBtn()
        {
            goStartList.SetActive(!goStartList.activeSelf);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
