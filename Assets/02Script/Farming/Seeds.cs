using System;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using UnityEngine;

namespace _02Script.Farming
{
    public class Seeds : MonoBehaviour
    {
        [SerializeField] private Fruit fruitPrefab;
        private SpriteRenderer mySpriteRenderer;
        
        [SerializeField] private SeedsSO mySO;
        [SerializeField] private Field myP;
        
        private CancellationTokenSource cts = new(); //시간을 위해

        private bool isSpawned;

        public void SetSO(SeedsSO seedsSO, Field field)
        {
            isSpawned = true;
            mySO = seedsSO;
            myP = field;
        }

        private async void OnEnable()
        {
            if(mySpriteRenderer == null)
                mySpriteRenderer = GetComponent<SpriteRenderer>();

            if (mySO == null)
            {
                await Task.Yield();
            }
            mySpriteRenderer.sprite = mySO.seeds.itemImage;
            
            if(isSpawned)
                _ = WaitGrow();
        }

        private async Task WaitGrow()
        {
            Fruit fruit = Instantiate(fruitPrefab);
            fruit.gameObject.SetActive(false);
            fruit.SetSO(mySO);
            fruit.transform.SetParent(gameObject.transform.parent);
            fruit.transform.position = gameObject.transform.position;
            
            
            await AsyncTime.WaitSeconds(mySO.growDelay, cts.Token);
            fruit.gameObject.SetActive(true);

            mySO = null;
            isSpawned = false;
            myP.ListSeeds(this);
            gameObject.SetActive(false);
        }

        private void Grow()
        {
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