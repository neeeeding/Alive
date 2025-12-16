using System;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Farming
{
    public class OneFarming : MonoBehaviour
    {
        [SerializeField] private Seeds seeds;
        [SerializeField] private Fruit fruit;
        [SerializeField] private SeedsGaugeUI seedsUI;
        
        [SerializeField] private SeedsSO mySO;
        [SerializeField] private Field myP;
        
        private CancellationTokenSource cts = new(); //시간을 위해

        private bool isSpawned;

        public void SetSO(SeedsSO so, Field field)
        {
            seeds.SetSO(so);
            fruit.SetSO(so, this);
            seedsUI.SetSO(so);
            isSpawned = true;
            mySO = so;
            myP = field;
        }

        private async void OnEnable()
        {
            fruit.gameObject.SetActive(false);
            seeds.gameObject.SetActive(true);
            seedsUI.gameObject.SetActive(true);

            if (mySO == null)
            {
                await Task.Yield();
            }

            if (isSpawned)
                _ = WaitGrow();
        }

        private async Task WaitGrow()
        {
            await AsyncTime.WaitSeconds(mySO.growDelay, cts.Token);
            fruit.gameObject.SetActive(true);
            seeds.gameObject.SetActive(false);
            seedsUI.gameObject.SetActive(false);

            isSpawned = false;
        }

        private void OnDisable()
        {
            mySO = null;
        }

        public void ListSeeds()
        {
            myP.ListSeeds(this);
        }

        private void OnDestroy()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}