using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private List<string> tutorialTexts = new List<string>();
        [Header("need")]
        [SerializeField] private TextMeshProUGUI tutorialTextUI;
        [SerializeField] private GameObject blackWindow; //검은 환경 
        [SerializeField] private GameObject skipBtn; 

        private int _curCount;

        private void Black()
        {
            blackWindow.SetActive(true);
            _curCount = 0;
        }

        private void Hide()
        {
            blackWindow.SetActive(false);
            tutorialTextUI.gameObject.SetActive(false);
        }

        public void Before()
        {
            _curCount = Mathf.Max(0, _curCount - 1);
            
            tutorialTextUI.text = tutorialTexts[_curCount];
        }

        public void Next()
        {
            if (tutorialTexts.Count <= ++_curCount)
            {
                EndTutorial();
            }
            tutorialTextUI.text = tutorialTexts[_curCount];
        }

        private void EndTutorial()
        {
            Hide();
            gameObject.SetActive(false);
        }

        public void TextShow(string text)
        {
            tutorialTextUI.gameObject.SetActive(true);
            tutorialTextUI.text = text;
        }
        
    }
}