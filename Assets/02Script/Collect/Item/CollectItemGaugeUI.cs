using System.Threading.Tasks;
using _02Script.Inventory.Item;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Collect.Item
{
    public class CollectItemGaugeUI : MonoBehaviour
    {
        [SerializeField] private Image gauge;
        [SerializeField] private Image itemImage;
        
        private float _curTime;
        private float _collectTime;
        private CollectItem _item;
        
        public void SetSO(ItemDataSO so, CollectItem item, float collectTime)
        {
            _collectTime = collectTime;
            
            itemImage.sprite = so.itemImage;
            _item = item;
            _curTime = 0;
            _ = WaitGrow();
        }

        private async Task WaitGrow()
        {
            while (_curTime < _collectTime)
            {
                if (!CollectItem.curSelectItem.ContainsKey(_item))
                {
                    await Task.Yield();
                    continue;
                }
                
                await Task.Yield();
                _curTime += Time.deltaTime;
                gauge.fillAmount = _curTime / _collectTime;
            }
        }
    }
}