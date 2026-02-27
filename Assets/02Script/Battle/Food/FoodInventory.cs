using System;
using System.Collections.Generic;
using _02Script.Collect.Item;
using _02Script.DoTweenUI.Warring;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _02Script.Battle.Food
{
    public class FoodInventory : InventoryManager
    {
        public static Action<EntityName,ItemDataSO,int> OnUseItem;
        
        [SerializeField] private GameObject inventoryWindow;
        private SerializedDictionary<ItemDataSO, List<int>>  _itemData = new SerializedDictionary<ItemDataSO, List<int>>();
        private Dictionary<ItemDataSO, List<EntityName>> _inventory = new Dictionary<ItemDataSO, List<EntityName>>();
        
        #region EnDiAw
        protected override void OnEnable()
        {
            CollectItem.OnGetItem += GetItem;
            FoodCheck.OnFood += Eat;
            FoodInventory.OnUseItem -= UseItem;
        }
        protected override void OnDisable()
        {
            CollectItem.OnGetItem -= GetItem;
            FoodCheck.OnFood -= Eat;
            FoodInventory.OnUseItem -= UseItem;
        }
        #endregion

        private void Eat(EntityName name, FoodInventoryCard card)
        {
            ItemData data = ItemDatas[card.ReturnData().ReturnDataSO()];
                
            int rand = Random.Range(1,6 -(int)card.ReturnNum(true));
            if(rand == 1)
            {
                WarringManager.Warring.ShowWarring("섭취에 성공하셨습니다!");
                BattleSaveManager.Instance.PlayerStat.characterStats[name][data.ReturnDataSO().stats] += data.ReturnDataSO().addStats;
            }
            else
            {
                WarringManager.Warring.ShowWarring("섭취에 실패하셨습니다...");
            }
            
            OnUseItem?.Invoke(_inventory[data.ReturnDataSO()][0], data.ReturnDataSO(),(int)card.ReturnNum(true));
            //data.UseItem((int)card.ReturnNum(true),true); //버리기
        }

        private void UseItem(EntityName name, ItemDataSO data, int count)
        {
            if (_itemData.ContainsKey(data))
            {
                _itemData[data].Remove(count);
                
                if(_itemData[data].Count <= 0)
                {
                    _itemData.Remove(data);
                    _inventory[data].Remove(name);
                    if (_inventory[data].Count <= 0)
                    {
                        _inventory.Remove(data);
                    }
                }
                ThrowItem(data,count);
            }
        }

        #region GetAddItem (inventory)

        private void GetItem(ItemDataSO data, int count, EntityName type) //아이템 얻은거, 카드도 생성
        {
            if(data.category != ItemCategory.food) return;
            
            if (!_itemData.ContainsKey(data))
            {
                _itemData.Add(data, new List<int> { 0 });
            }
            if (!_inventory.ContainsKey(data))
            {
                _inventory.Add(data, new List<EntityName> { });
            }
            if(!_inventory[data].Contains(type))
                _inventory[data].Add(type);

            if (data.category == ItemCategory.food ||
                data.category == ItemCategory.armor ||
                data.category == ItemCategory.weapon ||
                data.category == ItemCategory.machine)
            {
                _itemData[data].Add(count);
                AddItem(data,count);
            }
            else
            {
                _itemData[data][0] = count;
                for (int i = 0; i < count; i++)
                {
                    AddItem(data);
                }
            }
        }
        
        public override void AddItem(ItemDataSO item, int count = 1)
        {
            NewCard(item, ItemDatas.ContainsKey(item), count, count);

            ItemData data = ItemDatas[item];
            data.GetItem(count);
            
            ItemCard card = ItemCards[data][ItemCards[data].Count -1]; //갓 생성
            card.gameObject.SetActive(true);
        }
        #endregion
    }
}