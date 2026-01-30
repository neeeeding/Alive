using System;
using System.Threading.Tasks;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Player;
using _02Script.UI.Dialog.Entity;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Obj.Item
{
    public class CollectItem : GetItem
    {
        public static Action<CollectItem> OnClickItem;
        public static CollectItem curSelectItem;
        
        private readonly float[] value = {0.01f,0.05f,0.1f,0.25f,0.5f};

        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private int num; //개수 혹은 등급 || 내구도
        
        [Header("Need")]
        [SerializeField] private CollectItemGaugeUI gaugeUI;
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material outlineMaterial;
        
        private SpriteRenderer _renderer;
        private float _curS;
        private float _collectTime;



        #region EnDiAw
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            curSelectItem = null;
        }

        private void OnEnable()
        {
            CollectItem.OnClickItem += SelfCheck;
            _renderer.material = baseMaterial;
            gaugeUI.gameObject.SetActive(false);
            _curS = 0f;
        }

        private void OnDisable()
        {
            CollectItem.OnClickItem -= SelfCheck;
        }
        #endregion

        #region Select
        public void ClickItem()
        {
            if(CollectPlayerManager.curPlayer != null)
                OnClickItem?.Invoke(this);
        }

        private void SelfCheck(CollectItem item)
        {
            if (item != this && curSelectItem == this)
            {
                _renderer.material = baseMaterial;
            }
            else if(item == this)
            {
                _renderer.material = outlineMaterial;
                curSelectItem = null; //도착하면 캐도록
            }
        }
        #endregion

        public void SetItem(ItemDataSO data, int count)
        {
            itemData = data;
            num = count;
            gaugeUI.gameObject.SetActive(false);
        }

        #region gauge
        public void Gauge(EntityName characterName) //자원 얻기 시작 (게이지)
        {
            curSelectItem = this;
            if(_curS > 0) return;
            
            float stat = GameManager.Instance.PlayerStat.characterStats[characterName][StatsType.mining];
            float minus = 0;
            
            foreach (float v in value)
            {
                if(stat <= 1) break;
                minus += (Mathf.Min(stat, 10)-1) * v;
                stat -= (Mathf.Min(stat, 10)-1);
            }
            
            _collectTime = itemData.collectTime - minus;
            gaugeUI.gameObject.SetActive(true);
            gaugeUI.SetSO(itemData,this,_collectTime);
            _ = WaitCollect();
        }
        
        private async Task WaitCollect()
        {
            while (_curS < _collectTime)
            {
                if (curSelectItem != this)
                {
                    await Task.Yield();
                    continue;
                }
                    
                _curS += Time.deltaTime;
                await Task.Yield();
            }
            
            OnGetItem?.Invoke(itemData, num); //얘의 인벤토리 
            CollectItemManager.ItemBackList(this);
        }
        #endregion
    }
}