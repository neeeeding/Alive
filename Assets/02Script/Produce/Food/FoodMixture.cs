using System;
using _02Script.DoTweenUI.Warring;
using _02Script.MiniGame.Food;
using _02Script.MiniGame.Food.RiceCooker;
using UnityEngine;

namespace _02Script.Produce.Food
{
    public class FoodMixture : Mixture
    {
        [Header("Food")]
        [SerializeField] private GameObject miniGame;

        private int _getItemCount;

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
            miniGame.SetActive(true);

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

    }
}