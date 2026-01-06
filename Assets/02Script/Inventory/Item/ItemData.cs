using _02Script.DoTweenUI.Warring;
using _02Script.Manager;

namespace _02Script.Inventory.Item
{
    public class ItemData
    {
        private ItemDataSO _itemBaseData;

        private int _itemCount;

        public int ItemCount()
        {
            return _itemCount;
        }

        public ItemDataSO ReturnDataSO()
        {
            return _itemBaseData;
        }

        public void NewItem(ItemDataSO itemData)
        {
            _itemBaseData = itemData;
            _itemCount = 0;
        }

        public void UseItem(int use = 1, bool isThrow = false)
        {
            if (!isThrow && !_itemBaseData.DoSomething())
            {
                WarringManager.Warring.ShowWarring("인벤토리에서 사용할 수 있는 아이템이 아닙니다.");
                return;
            }
            
            _itemCount-=use;
            if (_itemCount <= 0)
            {
                _itemCount = 0;
            }
            GameManager.Instance.PlayerStat.items[_itemBaseData.itemType] =  _itemCount;
        }

        public void GetItem(int add = 1)
        {
            _itemCount+= add;
            if (_itemCount >= _itemBaseData.maxCount)//아이템만(부산물X)
            {
                _itemCount = _itemBaseData.maxCount;
            }
            GameManager.Instance.PlayerStat.items[_itemBaseData.itemType] =  _itemCount;
        }
    }
}