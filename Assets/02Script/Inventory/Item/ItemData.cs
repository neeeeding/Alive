using System.Collections.Generic;
using _02Script.Battle;
using _02Script.DoTweenUI.Warring;
using _02Script.GoHouse.Etc;
using _02Script.Manager;
using _02Script.Obj.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02Script.Inventory.Item
{
    public class ItemData
    {
        private ItemDataSO _itemBaseData;

        private int _itemCount;
        private readonly string _am = "AM_House";
        private readonly string _pm = "PM_Battle";
        private readonly string _mini = "GoHouse";

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
        public void UseItem(int use = 1, bool isThrow = false,EntityName name = EntityName.None)
        {
            if (!isThrow && !_itemBaseData.DoSomething(name))
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
                    if(SceneManager.GetActiveScene().name == _am)
                        HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    if(SceneManager.GetActiveScene().name == _mini)
                        GoHouseSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    if(SceneManager.GetActiveScene().name == _pm)
                        BattleSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    
                    break;
                
                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    _itemCount--;
                    if(SceneManager.GetActiveScene().name == _am)
                        HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType].Remove(use);
                    if(SceneManager.GetActiveScene().name == _mini)
                        GoHouseSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType].Remove(use);
                    if(SceneManager.GetActiveScene().name == _pm)
                        BattleSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType].Remove(use);
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
                    List<float> list = new List<float>();
                    if(SceneManager.GetActiveScene().name == _am)
                        list = HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType];
                    if(SceneManager.GetActiveScene().name == _mini)
                        list = GoHouseSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType];
                    if(SceneManager.GetActiveScene().name == _pm)
                        list = BattleSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType];

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
                    if(SceneManager.GetActiveScene().name == _am)
                        HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    if(SceneManager.GetActiveScene().name == _mini)
                        GoHouseSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    if(SceneManager.GetActiveScene().name == _pm)
                        BattleSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType][0] =  _itemCount;
                    
                    break;
                
                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    _itemCount++;
                    if(SceneManager.GetActiveScene().name == _am)
                        HouseManager.Instance.PlayerStat.items[_itemBaseData.itemType].Add(add);
                    if(SceneManager.GetActiveScene().name == _mini)
                        GoHouseSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType].Add(add);
                    if(SceneManager.GetActiveScene().name == _pm)
                        BattleSaveManager.Instance.PlayerStat.items[_itemBaseData.itemType].Add(add);
                    break;
            }
        }
        
        public void AddCountOnly()
        {
            _itemCount++;
        }

        public void SetCountOnly(int count)
        {
            _itemCount = count;
        }
    }
}