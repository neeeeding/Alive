using System;
using _02Script.Inventory.Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.UI.Store
{
    public class Store : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] protected int sellCount;
        [SerializeField] protected ItemDataSO[] sellDataSos;
        [SerializeField] protected ItemDataSO[] payDataSos;
        [Header("Need")]
        [SerializeField] protected StoreCard cardPrefab;
        [SerializeField] protected Transform parent;

        protected int CardIndex;

        private void Awake()
        {
            SettingStore();
        }


        protected void SettingStore()
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(0).gameObject);
            }
            
            CardIndex = -1;
            for (int i = 0; i < sellCount; i++)
            {
                StoreCard card = Instantiate(cardPrefab, parent);

                SetCardIndex(false);
                int sell = CardIndex;
                SetCardIndex(true);
                int pay = CardIndex;

                ItemCategory category = sellDataSos[sell].category;
                
                //내구도 있는 것들만  80 ~ 100 내구도, 나머지는 1 ~ 3개 (요리도)
                card.SetCard(sellDataSos[sell], payDataSos[pay], 
                    category == ItemCategory.armor || category == ItemCategory.machine ||
                    category == ItemCategory.stuff ? Random.Range(80,101):
                    Random.Range(1,4), Random.Range(1,6));
            }
        }

        protected virtual void SetCardIndex(bool isPay)
        {
            if(isPay) return;
            CardIndex++;
        }
    }
}