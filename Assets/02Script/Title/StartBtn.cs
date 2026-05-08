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

        public void NewGame()
        {
            GameSaveData data = new GameSaveData();
            data.DataReset();
            data.stat.ResetStat();
                
            HouseManager.Instance.PlayerStat = data.stat; //로드
            
            PlayerPrefs.SetString(HouseManager.GameSaveFilePath,"");
            PlayerPrefs.Save();
            SceneManager.LoadScene(startScene);   
        }
        
        public void StartGame()
        {
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