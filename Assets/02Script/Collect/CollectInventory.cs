using System.Collections.Generic;
using _02Script.Battle;
using _02Script.Battle.Food;
using _02Script.Collect.Item;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace _02Script.Collect
{
    public class CollectInventory : InventoryManager
    {
        [Header("Setting")]
        [SerializeField] private EntityName inventoryCharacter;
        [Header("Need")]
        [SerializeField] private TextMeshProUGUI inventoryText;

        private string _characterName;
        private int _inventoryMaxCount;
        private int _curCount;
        private SerializedDictionary<ItemDataSO, List<int>>  _itemData = new SerializedDictionary<ItemDataSO, List<int>>();

        #region EnDiAw
        protected override void OnEnable()
        {
            CollectItem.OnGetItem += GetItem;
            _curCount = 0;
            BattleSaveManager.OnStart += SetStart;
            FoodInventory.OnUseItem += UseItem;
            DeleteBtn.OnDelete += Delete;
        }

        protected override void OnDisable()
        {
            CollectItem.OnGetItem -= GetItem;
            BattleSaveManager.OnStart -= SetStart;
            FoodInventory.OnUseItem -= UseItem;
            DeleteBtn.OnDelete += Delete;
        }
        #endregion

        #region GetAddItem (inventory)

        private void GetItem(ItemDataSO data, int count, EntityName type) //아이템 얻은거, 카드도 생성
        {
            if(inventoryCharacter != type) return;
            
            if (!_itemData.ContainsKey(data))
            {
                _itemData.Add(data, new List<int> { 0 });
            }

            if (data.category == ItemCategory.food ||
                data.category == ItemCategory.armor ||
                data.category == ItemCategory.weapon ||
                data.category == ItemCategory.machine)
            {
                if (!ItemCountAddCanCheck()) return;
                _itemData[data].Add(count);
                AddItem(data,count);
            }
            else
            {
                _itemData[data][0] = count;
                for (int i = 0; i < count; i++)
                {
                    if(!ItemCountAddCanCheck()) return;
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
            (card as CollectInventoryCard).SetCharacter(inventoryCharacter);
            card.gameObject.SetActive(true);
        }
        #endregion

        private void Delete(EntityName name, ItemDataSO so, float count)
        {
            if(name != inventoryCharacter) return;
            ThrowItem(so,(int)count);
        }

        private void UseItem(EntityName name, ItemDataSO data, int count)
        {
            if(inventoryCharacter != name) return;
            
            ThrowItem(data,count);
        }

        protected override void LessItem(ItemDataSO item, bool isThrow, int count = 1)
        {
            base.LessItem(item, isThrow, count);
            _curCount--;
        }

        private bool ItemCountAddCanCheck()
        {
            if (_curCount >= _inventoryMaxCount)
            {
                WarringManager.Warring.ShowWarring(_characterName+"의 인벤토리가 가득찼습니다.");
                return false;
            }
            _curCount++;
            return true;
        }

        #region Set
        private void SetStart()
        {
            if (inventoryCharacter != EntityName.None)
            {
                SetInventoryMaxCount();
            }
        }

        public void SetInventoryCharacter(EntityName type) //누구의 인벤토리인지 지정해주기
        {
            inventoryCharacter = type;
            SetInventoryMaxCount();
        }

        private void SetInventoryMaxCount()
        {
            _inventoryMaxCount = (int)StatCalculate.Calculate(inventoryCharacter,StatsType.acceptance);
            
            _characterName = EnumToString.Name(inventoryCharacter);
            inventoryText.text = _characterName+"의 인벤토리";
        }
        #endregion
    }
}