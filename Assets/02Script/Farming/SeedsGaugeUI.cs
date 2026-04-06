using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Farming
{
    public class SeedsGaugeUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiObj;
        [SerializeField] private Image gauge;
        [SerializeField] private Image viandImage;
        
        private float curTime;
        private float growTime;
        
        public void SetSO(SeedsSO seedsSO, float time)
        {
            growTime = seedsSO.growDelay;
            
            viandImage.sprite = seedsSO.viand.itemImage;
            curTime = time;
            _ = WaitGrow();
        }

        public void ShowUI()
        {
            uiObj.SetActive(!uiObj.activeSelf);
        }

        private void OnEnable()
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