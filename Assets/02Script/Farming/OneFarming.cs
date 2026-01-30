using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Farming
{
    public class OneFarming : MonoBehaviour
    {
        [SerializeField] private Seeds seeds;
        [SerializeField] private Viand viand;
        [SerializeField] private SeedsGaugeUI seedsUI;
        
        [SerializeField] private SeedsSO mySO;
        [SerializeField] private Field myP;
        
        private CancellationTokenSource cts = new(); //시간을 위해

        private bool _isSpawned;

        public SeedsSO GetSO()
        {
            return mySO;
        }

        public void SetSO(SeedsSO so, Field field)
        {
            seeds.SetSO(so);
            viand.SetSO(so, this);
            seedsUI.SetSO(so);
            _isSpawned = true;
            mySO = so;
            myP = field;
        }

        private async void OnEnable()
        {
            viand.gameObject.SetActive(false);
            seeds.gameObject.SetActive(true);
            seedsUI.gameObject.SetActive(true);

            if (mySO == null)
            {
                await Task.Yield();
            }

            if (_isSpawned)
                _ = WaitGrow();
        }

        private async Task WaitGrow()
        {
            await AsyncTime.WaitSeconds(mySO.growDelay, cts.Token, false);
            viand.gameObject.SetActive(true);
            seeds.gameObject.SetActive(false);
            seedsUI.gameObject.SetActive(false);

            _isSpawned = false;
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