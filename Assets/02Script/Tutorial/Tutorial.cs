using System.Collections.Generic;
using _02Script.Manager;
using _02Script.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _02Script.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [Header("need")]
        [SerializeField] protected TextMeshProUGUI tutorialTextUI;
        [SerializeField] protected GameObject blackWindow; //검은 환경 
        [SerializeField] protected GameObject skipBtn; 

        protected List<(string text,bool isStop/*시간 멈출 건지*/)> tutorialDetail = new List<(string,bool)>();
        protected int _curCount;
        [SerializeField] private string startScene = "AM_House";

        protected virtual void Black()
        {
            blackWindow.SetActive(true);
            _curCount = 0;
        }

        protected virtual void Hide()
        {
            blackWindow.SetActive(false);
            tutorialTextUI.gameObject.SetActive(false);
        }

        public virtual void Before()
        {
            _curCount = Mathf.Max(0, _curCount - 1);
            ChangeText();
        }

        public virtual void Next()
        {
            if (tutorialDetail.Count <= ++_curCount)
            {
                EndTutorial();
                return;
            }
            ChangeText();
        }

        protected virtual void ChangeText()
        {
            tutorialTextUI.text = tutorialDetail[_curCount].text;
            Time.timeScale = tutorialDetail[_curCount].isStop ? 0 : 1;
            blackWindow.SetActive(tutorialDetail[_curCount].isStop);
        }

        protected virtual void EndTutorial()
        {
            Hide();
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public virtual void TextShow(string text)
        {
            tutorialTextUI.gameObject.SetActive(true);
            tutorialTextUI.text = text;
        }

        public virtual void ResetGame()
        {
            GameSaveData data = new GameSaveData();
            data.DataReset();
            data.stat.ResetStat();
                
            HouseManager.Instance.PlayerStat = data.stat; //로드
            
            PlayerPrefs.SetString(HouseManager.GameSaveFilePath,"");
            PlayerPrefs.Save();
            SceneManager.LoadScene(startScene);
            SceneManager.LoadScene(startScene);
        }
        
    }
}