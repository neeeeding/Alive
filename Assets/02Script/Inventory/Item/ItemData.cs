using System.Collections.Generic;
using _02Script.DoTweenUI.Warring;
using _02Script.Manager;
using UnityEngine;

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

        //내구도가 아닌 하나의 아이템으로 봤을 경우
        public void UseItem(int use = 1, bool isThrow = false)
        {
            if (!isThrow && !_itemBaseData.DoSomething())
            {
                WarringManager.Warring.ShowWarring("인벤토리에서 사용할 수 있는 아이템이 아닙니다.");
                return;
            }
            
            switch(_itemBaseData.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    
                    _itemCount-=use;
                    if (_itemCount <= 0)
                    {
                        _itemCount = 0;
                    }
                    HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    
                    break;
                
                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    _itemCount--;
                    HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType].Remove(use);
                    break;
            }
            
        }

        //내구도 닳는용
        public void UseItem(float use,float minus = 1, bool isThrow = false)
        {
            switch(_itemBaseData.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    break;
                
                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    List<float> list = HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType];

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] != use) continue;

                        list[i] -= minus;

                        if (list[i] <= 0)
                        {
                            _itemCount--;
                            list.RemoveAt(i);
                        }
                        break;
                    }
                    break;
            }
            
        }

        public void GetItem(int add = 1)
        {
            switch(_itemBaseData.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    
                    _itemCount+= add;
                    if (_itemCount >= _itemBaseData.maxCount)//아이템만(부산물X)
                    {
                        _itemCount = _itemBaseData.maxCount;
                    }
                    HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    
                    break;
                
                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    _itemCount++;
                    HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType].Add(add);
                    break;
            }
        }
    }
}