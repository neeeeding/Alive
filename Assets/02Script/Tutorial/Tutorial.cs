using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
            gameObject.SetActive(false);
        }

        public virtual void TextShow(string text)
        {
            tutorialTextUI.gameObject.SetActive(true);
            tutorialTextUI.text = text;
        }
        
    }
}