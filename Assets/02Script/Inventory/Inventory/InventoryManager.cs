using System.Collections.Generic;
using System.Linq;
using _02Script.Farming;
using _02Script.Inventory.Inventory.Use;
using _02Script.Inventory.Item;
using _02Script.Produce.Weapon;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Store;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected ItemCard cardPrefab;
        [SerializeField] protected SerializedDictionary<ItemCategory, Transform> itemInventory;
        
        protected Dictionary<ItemDataSO, ItemData> ItemDatas = new Dictionary<ItemDataSO, ItemData>();
        protected Dictionary<ItemData, List<ItemCard>> ItemCards = new Dictionary<ItemData, List<ItemCard>>();
        
        //hold에 대해
        [SerializeField]private ItemHold realItem; //들리게 될 아이템(위치)

        #region EnDi
        protected virtual void OnEnable()
        {
            DialogItem.OnGetItem += GetOrThrowItem;
            InGameItem.OnGetItem += AddItem;
            Field.OnGetViand += AddItem;
            GameEvent.GameEvent.OnGetItem += AddItem;
            StoreCard.OnSellItem += AddItem;
            StoreCard.OnPayItem += ThrowItem;
            Field.OnUseSeed += ThrowItem;
            
            UseWindow.OnHold += HoldItem;
            UseWindow.OnUse += UseItem;
            UseWindow.OnThrow += ThrowItem;
        }

        protected virtual  void OnDisable()
        {
            DialogItem.OnGetItem -= GetOrThrowItem;
            InGameItem.OnGetItem -= AddItem;
            Field.OnGetViand -= AddItem;
            GameEvent.GameEvent.OnGetItem -= AddItem;
            StoreCard.OnSellItem -= AddItem;
            StoreCard.OnPayItem -= ThrowItem;
            Field.OnUseSeed -= ThrowItem;
            
            UseWindow.OnHold -= HoldItem;
            UseWindow.OnUse -= UseItem;
            UseWindow.OnThrow -= ThrowItem;
        }
        #endregion

        public bool FindItem(ItemDataSO item)
        {
            return ItemDatas.ContainsKey(item) && ItemDatas[item].ItemCount() > 0;
        }

        public void GetOrThrowItem(ItemDataSO item,int count)
        {
            if (count > 0)
            {
                AddItem(item, count);
            }
            else
            {
                count*= -1;
                ThrowItem(item, count);
            }
        }
        
        public void HoldItem(ItemData item, int count = 1) //들기
        {
            realItem.Setting(item, count);
        }

        public void UseItem(ItemDataSO item, int count = 1) //사용
        {
            LessItem(item, false, count);
        }

        public void ThrowItem(ItemDataSO item, int count = 1) //버리기
        {
            LessItem(item, true, count);
        }

        protected virtual void LessItem(ItemDataSO item, bool isThrow,int count = 1,WeaponArmorSaveData saveData = null) //어쨌든 아이템 감소
        {
            if (ItemDatas.ContainsKey(item))
            {
                ItemData data = ItemDatas[item];
                
                data.UseItem(count, isThrow);
                ItemCards[data][ItemCards[data].Count -1].UpdateCountUI();
                
                if(realItem != null)
                    realItem.CheckLessItem();
                
                switch(item.category) //개별 저장 애들은 걍 지워버리기
                {
                    case ItemCategory.food:
                        foreach (ItemCard card in ItemCards[data].ToList())
                        {
                            if(count != card.ReturnNum(true)) continue;
                            
                            ItemCards[data].Remove(card);
                            Destroy(card.gameObject);
                            break;
                        }
                        break;
                    case ItemCategory.armor:
                    case ItemCategory.weapon:
                    case ItemCategory.machine:
                        foreach (ItemCard card in ItemCards[data].ToList())
                        {
                            print($"{gameObject.name} / {transform.parent.name} => {item.name}");
                            if(count != card.ReturnNum(false)) continue;
                            
                            ItemCards[data].Remove(card);
                            Destroy(card.gameObject);
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        protected virtual void NewCard(ItemDataSO item, bool isEtc, int star = 3, int hp = 100,WeaponArmorSaveData saveData = null )
        {
            if(!itemInventory.ContainsKey(item.category)) return;
            //data 새 생성
            ItemData itemData = new ItemData();
            if (!isEtc)
            {
                itemData.NewItem(item);
                ItemDatas.Add(item, itemData);
            }
            else //기존거
            {
                itemData = ItemDatas[item];
            }
            
            Transform parent = itemInventory[item.category];
            
            //카드 새 생성
            ItemCard newCard = Instantiate(cardPrefab, parent);
            newCard.gameObject.SetActive(true);
            newCard.NewCard(itemData, star, hp,saveData);
            
            if(!isEtc)
                ItemCards.Add(itemData, new List<ItemCard>());

            ItemCards[itemData].Add(newCard);
        }

        public virtual void AddItem(ItemDataSO item,WeaponArmorSaveData saveData, int count = 1)
        {
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
            data.GetItem(count);
            
            ItemCard card = ItemCards[data][ItemCards[data].Count -1]; //갓 생성
            card.gameObject.SetActive(true);
            card.UpdateCountUI();
        }

        public virtual void AddItem(ItemDataSO item, int count = 1)
        {
            if(!itemInventory.ContainsKey(item.category)) return;
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
                    NewCard(item, ItemDatas.ContainsKey(item), count, count);
                    break;
            }

            if (!ItemDatas.ContainsKey(item))
            {
                return;
            }

            ItemData data = ItemDatas[item];
            data.GetItem(count);
            
            ItemCard card = ItemCards[data][ItemCards[data].Count -1]; //갓 생성
            card.gameObject.SetActive(true);
            card.UpdateCountUI();
        }
    }
}