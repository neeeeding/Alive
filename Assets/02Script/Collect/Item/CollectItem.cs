using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _02Script.Battle.UI.Etc;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.GamePlayer.Manager;
using _02Script.UI.person;
using UnityEngine;

namespace _02Script.Collect.Item
{
    public class CollectItem : MonoBehaviour
    {
        public static Action<ItemDataSO, int,EntityName> OnGetItem;
        public static Action<CollectItem> OnClickItem;
        public static Dictionary<CollectItem,EntityName> curSelectItem = new Dictionary<CollectItem,EntityName>();

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
        }

        private void OnEnable()
        {
            CollectItem.OnClickItem += SelfCheck;
            GameMiddleUI.OnEndCollect += EndCollectGame;
            _renderer.material = baseMaterial;
            gaugeUI.gameObject.SetActive(false);
            _curS = 0f;
        }

        private void OnDisable()
        {
            CollectItem.OnClickItem -= SelfCheck;
            GameMiddleUI.OnEndCollect -= EndCollectGame;
        }
        #endregion

        #region Select
        public void ClickItem()
        {
            if (CollectPlayerManager.curPlayer != null)
            {
                if (curSelectItem.ContainsKey(this))
                {
                    WarringManager.Warring.ShowWarring($"이미 {EnumToString.Name(curSelectItem[this])}이/가 수집 중입니다.");
                }
                else
                    OnClickItem?.Invoke(this);
            }
        }

        private void SelfCheck(CollectItem item)
        {
            if (item != this && curSelectItem.ContainsKey(this))
            {
                _renderer.material = baseMaterial;
                foreach (KeyValuePair<CollectItem, EntityName> kvp in curSelectItem.ToArray())
                {
                    if (kvp.Value == CollectPlayerManager.curPlayer.playerName)
                    {
                        curSelectItem.Remove(kvp.Key);
                        break;
                    }
                }
            }
            else if(item == this)
            {
                _renderer.material = outlineMaterial;
                curSelectItem.Remove(this); //도착하면 캐도록
            }
        }
        #endregion

        public void SetItem(ItemDataSO data, int count)
        {
            itemData = data;
            num = count;
            gaugeUI.gameObject.SetActive(false);
        }

        private void EndCollectGame()
        {
            if (!curSelectItem.ContainsKey(this)) return;

            curSelectItem.Remove(this);
        }

        #region gauge
        public void Gauge(EntityName characterName) //자원 얻기 시작 (게이지)
        {
            if (curSelectItem.ContainsValue(characterName))
            {
                foreach (KeyValuePair<CollectItem, EntityName> kvp in curSelectItem.ToArray())
                {
                    if (kvp.Value == characterName)
                    {
                        curSelectItem.Remove(kvp.Key);
                        break;
                    }
                }
            }
            
            if(!curSelectItem.ContainsKey(this))
                curSelectItem.Add(this,characterName);
            if(_curS > 0) return;
            
            float minus = StatCalculate.Calculate(characterName, StatsType.mining);
            
            _collectTime = itemData.collectTime - minus;
            gaugeUI.gameObject.SetActive(true);
            gaugeUI.SetSO(itemData,this,_collectTime);
            _ = WaitCollect();
        }
        
        private async Task WaitCollect()
        {
            while (_curS < _collectTime)
            {
                if (!curSelectItem.ContainsKey(this))
                {
                    await Task.Yield();
                    continue;
                }
                    
                _curS += Time.deltaTime;
                await Task.Yield();
            }
            
            OnGetItem?.Invoke(itemData, num,curSelectItem[this]);
            curSelectItem.Remove(this);
            CollectItemManager.ItemBackList(this);
        }
        #endregion
    }
}