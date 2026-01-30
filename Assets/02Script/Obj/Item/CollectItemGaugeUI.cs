using System.Threading.Tasks;
using _02Script.Inventory.Item;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Obj.Item
{
    public class CollectItemGaugeUI : MonoBehaviour
    {
        [SerializeField] private Image gauge;
        [SerializeField] private Image itemImage;
        
        private float _curTime;
        private float _collectTime;
        
        public void SetSO(ItemDataSO so)
        {
            _collectTime = so.collectTime;
            
            itemImage.sprite = so.itemImage;
            _curTime = 0;
            _ = WaitGrow();
        }

        private async Task WaitGrow()
        {
            while (_curTime < _collectTime)
            {
                await Task.Yield();
                _curTime += Time.deltaTime;
                gauge.fillAmount = _curTime / _collectTime;
            }
        }
    }
}