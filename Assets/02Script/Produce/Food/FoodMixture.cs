using _02Script.DoTweenUI.Warring;
using _02Script.MiniGame.Food;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using UnityEngine;

namespace _02Script.Produce.Food
{
    public class FoodMixture : Mixture
    {
        [Header("Food")]
        [SerializeField] private SerializedDictionary<MeansDataSO,GameObject> miniGame;

        private int _getItemCount;

        private MeansDataSO _curMeans;

        protected override void OnEnable()
        {
            base.OnEnable();
            FoodScore.OnEndMiniGame += GetResult;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            FoodScore.OnEndMiniGame -= GetResult;
        }

        #region Btn
        public override void MouseEnter() // 누르는 시간 재기 시작
        {
            _timer = setTimer;
            _isTimer = true;
        }
        public override void MouseExit() //누른 시간에 따라 전부 or 하나만 얻을지
        {
            _isTimer = false;
            if (!_isCanUse)
            {
                WarringManager.Warring.ShowWarring(_warringText);
                return;
            }
            if(_curMeans != null)
                miniGame[_curMeans].SetActive(true);

            _getItemCount = _timer <= 0 ? _itemMax.maxCount : 1;
        }
        protected override void GetResult(int count) //최대 5
        {
            int use = _getItemCount;
            foreach (var card in _cards)
            {
                card.ReturnData().UseItem(use,true);
                card.UpdateCountUI();
                
                inventory.ThrowItem(card.ReturnData().ReturnDataSO(),use);
            }
            inventory.AddItem(resultData,count);
            produceInventory.AddItem(resultData,count);
            _itemMax.maxCount -= use;
            
            resultCount.text = _itemMax.maxCount > 0? _itemMax.maxCount.ToString() : "";
            errorMassage.SetActive(_itemMax.maxCount <= 0);
        }
        #endregion

        protected override void Setting(ProduceBookSO bookData)
        {
            if (bookData == null || bookData.means == null)
            {
                _curMeans = null;
            }
            else
                _curMeans = bookData.means;
            base.Setting(bookData);
        }
    }
}