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
        [SerializeField] private Image fruitImage;
        
        private float curTime;
        private float growTime;
        
        public void SetSO(SeedsSO seedsSO)
        {
            growTime = seedsSO.growDelay;
            
            fruitImage.sprite = seedsSO.fruit.itemImage;
            curTime = 0;
            _ = WaitGrow();
        }

        public void ShowUI()
        {
            uiObj.SetActive(!uiObj.activeSelf);
        }

        private async void OnEnable()
        {
            uiObj.SetActive(false);
        }

        private async Task WaitGrow()
        {
            while (curTime < growTime)
            {
                await Task.Yield();
                curTime += Time.deltaTime;
                gauge.fillAmount = curTime / growTime;
            }
        }
    }
}