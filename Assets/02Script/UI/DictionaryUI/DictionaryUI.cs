using System.Collections.Generic;
using _02Script.Manager;
using _02Script.UI.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.UI.DictionaryUI
{
    public class DictionaryUI : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private Sprite[] images;
        [TextArea(3, 10)]
        [SerializeField] private string[] texts;
        [Header("Need")]
        [SerializeField] private Image dictionaryImage;
        [SerializeField] private TextMeshProUGUI dictionaryText;
        [SerializeField] private TMP_InputField memoInputField;

        //0 부터
        private List<int> _getPage;
        private List<string> _memo;
        private int _curPage;

        #region EnDiAw
        private void OnEnable()
        {
            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            LoadCard.OnLoad -= Load;
        }

        private void Awake()
        {
            Load();   
        }

        #endregion

        public void SaveMemo()
        {
            _memo[_curPage] = memoInputField.text;
            HouseManager.Instance.PlayerStat.getDictionaryPageMemo[_curPage] = memoInputField.text;
        }

        public void ShowUI()
        {
            UISettingManager.Instance.UIDictionary();
            _curPage = 0;
            ShowDictionary();
        }

        public void AfterBtn()
        {
            if (_curPage <= 0)
            {
                _curPage = 0;
            }

            _curPage--;
            ShowDictionary();
        }

        public void BeforeBtn()
        {
            if (_curPage >= _getPage.Count -1)
            {
                _curPage = _getPage.Count -1;
            }

            _curPage++;
            ShowDictionary();
        }

        private void ShowDictionary()
        {
            dictionaryImage.sprite = images[_curPage];
            dictionaryText.text = texts[_curPage];
            memoInputField.text = _memo[_curPage];
        }

        public void GetPage(int page, string text ="", Sprite image = null)
        {
            if (_getPage.Count <= 0)
            {
                Load();
            }
            if (_getPage[page] > 0)
            {
                if (string.IsNullOrEmpty(text))
                {
                    AddText(page, text, image);
                }
                return;
            }
            _getPage[page] = 1;
            HouseManager.Instance.PlayerStat.getDictionaryPage[page] = 1;
        }
        
        private void AddText(int page, string text, Sprite image = null)
        {
            texts[page] = text;
            
            if(image == null) return;
            images[page] = image;
        }
        
        private void Load()
        {
            _getPage = HouseManager.Instance.PlayerStat.getDictionaryPage;
            _memo = HouseManager.Instance.PlayerStat.getDictionaryPageMemo;

            if (_getPage.Count <= 0)
            {
                _getPage = new List<int>();
                for (int i = 0; i < texts.Length; i++)
                {
                    _getPage.Add(i);
                    _memo.Add("");
                }
                HouseManager.Instance.PlayerStat.getDictionaryPage = _getPage;
                HouseManager.Instance.PlayerStat.getDictionaryPageMemo = _memo;
            }
        }
    }
}