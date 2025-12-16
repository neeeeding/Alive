using System;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Farming
{
    public class SeedsGaugeUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiObj;
        [SerializeField] private Image gauge;
        [SerializeField] private SeedsSO mySO;
        [SerializeField] private Image fruitImage;
        
        private CancellationTokenSource cts = new(); //시간을 위해
        private float curTime;
        
        public void SetSO(SeedsSO seedsSO)
        {
            mySO = seedsSO;
            curTime = 0;
        }

        public void ShowUI()
        {
            uiObj.SetActive(!uiObj.activeSelf);
        }

        private async void OnEnable()
        {
            uiObj.SetActive(false);

            if (mySO == null)
            {
                await Task.Yield();
            }
            
            fruitImage.sprite = mySO.fruit.itemImage;
            
            _ = WaitGrow();
        }

        private async Task WaitGrow()
        {
            while (curTime < mySO.growDelay)
            {
                await AsyncTime.WaitSeconds(0.01f, cts.Token);
                curTime += 0.01f;
                gauge.fillAmount = curTime / mySO.growDelay;
            }
        }
        protected virtual void OnDestroy()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}