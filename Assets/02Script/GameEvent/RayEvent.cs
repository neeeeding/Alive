using System.Collections.Generic;
using _02Script.Manager;
using _02Script.UI.DictionaryUI;
using _02Script.UI.Save;
using _02Script.UI.Store;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.GameEvent
{
    public class RayEvent : Store
    {
        [Space(50)]
        [Header("Letter")]
        [SerializeField] private List<int> pageList;
        [SerializeField] private List<string> changeText;
        [SerializeField] private List<Sprite> changeImage;
        [SerializeField] private RayLetter letter;
        [SerializeField] private DictionaryUI dictionaryUI;

        private int _currPage;

        private void OnEnable()
        {
            Load();
            SettingStore();
            dictionaryUI.GetPage(pageList[_currPage], changeText[_currPage], changeImage[_currPage]);

            LoadCard.OnLoad += Load;
        }

        private void OnDisable()
        {
            LoadCard.OnLoad -= Load;
        }

        //편지 관련 -----------------------------------------------------------------------------------------------
        public void ClickLetter()
        {
            letter.ShowLetter(_currPage);
        }

        private void Load()
        {
            _currPage = GameManager.Instance.PlayerStat.day / 2;
            _currPage -= GameManager.Instance.PlayerStat.day / 8;
        }
        
        // 상점 관련  -----------------------------------------------------------------------------------------------
        protected override void SetCardIndex(bool isPay)
        {
            CardIndex = Random.Range(0, isPay ? payDataSos.Length :  sellDataSos.Length);
        }
    }
}