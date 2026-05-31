using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.MiniGame
{
    public class MiniGameExBook : MonoBehaviour
    {
        [SerializeField] protected GameObject timer;
        [SerializeField] protected GameObject book;
        [SerializeField] protected List<Sprite> images;
        
        [SerializeField] protected TextMeshProUGUI text;
        [SerializeField] protected Image image;

        [SerializeField] protected List<string> _tutorialDetail;
        protected int _curCount;

        private void OnEnable()
        {
            _curCount = 0;
            ShowText();
        }

        protected virtual void ShowText()
        {
            book.SetActive(true);
            Time.timeScale = 0;
            text.gameObject.SetActive(true);
            text.text = _tutorialDetail[_curCount];

            if (images[_curCount] != null)
            {
                image.gameObject.SetActive(true);
                image.sprite = images[_curCount];
            }
        }

        public void Next()
        {
            _curCount++;

            if (_curCount == _tutorialDetail.Count)
            {
                _curCount--;
                timer.gameObject.SetActive(true);
                book.SetActive(false);
                gameObject.SetActive(false);
                return;
            }
            ShowText();
        }
    }
}