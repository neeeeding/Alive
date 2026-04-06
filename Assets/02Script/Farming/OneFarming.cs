using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
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
        private float _curTime;
        private SaveVector2 _curFarmPos;

        public SeedsSO GetSO()
        {
            return mySO;
        }

        public void SetSO(SeedsSO so,Vector3 farmPos, Field field)
        {
            _curFarmPos = (Vector2)farmPos;
            
            if (!HouseManager.Instance.PlayerStat.farm.ContainsKey(_curFarmPos))
            {
                HouseManager.Instance.PlayerStat.farm.Add(_curFarmPos, so.seeds.itemType);
            }
            HouseManager.Instance.PlayerStat.farm[_curFarmPos] = so.seeds.itemType;
            
            _curTime = 0;
            if (HouseManager.Instance.PlayerStat.farmTime.ContainsKey(_curFarmPos))
            {
                _curTime = HouseManager.Instance.PlayerStat.farmTime[_curFarmPos];
            }            
            seeds.SetSO(so);
            viand.SetSO(so, this);
            seedsUI.SetSO(so,_curTime);
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
            while (_curTime < mySO.growDelay)
            {
                await AsyncTime.WaitSeconds(1f, cts.Token, false);
                _curTime += 1f;
                if (!HouseManager.Instance.PlayerStat.farmTime.ContainsKey(_curFarmPos))
                {
                    HouseManager.Instance.PlayerStat.farmTime.Add(_curFarmPos,_curTime);
                }
                HouseManager.Instance.PlayerStat.farmTime[_curFarmPos] = _curTime;
                
            }
            viand.gameObject.SetActive(true);
            seeds.gameObject.SetActive(false);
            seedsUI.gameObject.SetActive(false);
            if (!HouseManager.Instance.PlayerStat.farmTime.ContainsKey(_curFarmPos))
            {
                HouseManager.Instance.PlayerStat.farmTime.Add(_curFarmPos,0);
            }
            HouseManager.Instance.PlayerStat.farmTime[_curFarmPos] = 0;

            _isSpawned = false;
        }
        public void ListSeeds()
        {
            HouseManager.Instance.PlayerStat.farmTime[_curFarmPos] = 0;
            HouseManager.Instance.PlayerStat.farm[_curFarmPos] = ItemType.none;
            myP.ListSeeds(this);
            mySO = null;
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