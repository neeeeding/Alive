using System;
using System.Threading;
using System.Threading.Tasks;
using _02Script.Etc;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Obj.Item
{
    public class CollectItem : GetItem
    {
        public static Action<CollectItem> OnClickItem;

        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private int num; //개수 혹은 등급 || 내구도
        [SerializeField] private CollectItemGaugeUI gaugeUI;
        
        private CollectItemManager _manager;
        private CancellationTokenSource _cts = new(); //시간을 위해

        public void ClickItem()
        {
            OnClickItem?.Invoke(this);
            //아웃라인 만들어주기 (주석)
        }

        public void SetItem(ItemDataSO data, int count, CollectItemManager manager)
        {
            itemData = data;
            num = count;
            _manager = manager;
            gaugeUI.gameObject.SetActive(false);
        }

        public async void Gauge() //자원 얻기 시작 (게이지)
        {
            gaugeUI.gameObject.SetActive(true);
            gaugeUI.SetSO(itemData);
            _ = WaitCollect();
        }
        
        private async Task WaitCollect()
        {
            await AsyncTime.WaitSeconds(itemData.collectTime, _cts.Token, false);
            OnGetItem?.Invoke(itemData, num); //얘의 인벤토리 
            _manager.ItemBackList(this);
        }

        private void OnDisable()
        {
            gaugeUI.gameObject.SetActive(false);
        }
    }
}