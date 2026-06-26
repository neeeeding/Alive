using System;
using _02Script.Inventory.Item;
using _02Script.Produce.Weapon;
using _02Script.Produce.Weapon.Compound;
using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class HouseInventoryManager : LoadInventoryManager
    {
        [SerializeField] private bool isJustShow;
        protected override void OnEnable()
        {
            ProduceResult.OnGetItem += AddItem;
            ProduceResult.OnUseItem += ThrowItem;
            CompoundResult.OnGetItem += AddItem;
            CompoundResult.OnUseItem += ThrowItem;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            ProduceResult.OnGetItem -= AddItem;
            ProduceResult.OnUseItem -= ThrowItem;
            CompoundResult.OnGetItem -= AddItem;
            CompoundResult.OnUseItem -= ThrowItem;
            base.OnDisable();
        }

        public override void AddItem(ItemDataSO item,WeaponArmorSaveData saveData, int count = 1)
        {
            if(!isJustShow) {base.AddItem(item,saveData, count);return;}
            
            item = AllDataSO[item.itemType];
            
            switch(item.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    if (!ItemDatas.ContainsKey(item))
                        NewCard(item, false, 0,0);
                    break;

                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    NewCard(item, ItemDatas.ContainsKey(item), count, count,saveData);
                    break;
            }

            ItemData data = ItemDatas[item];
            
            ItemCard card = ItemCards[data][ItemCards[data].Count -1]; //갓 생성
            card.gameObject.SetActive(true);
            card.UpdateCountUI();
        }
    }
}