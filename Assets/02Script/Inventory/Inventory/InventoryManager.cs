using System.Collections.Generic;
using System.Linq;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.UI.Save;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using ItemCard = _02Script.Inventory.Item.ItemCard;

namespace _02Script.Inventory.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected ItemCard cardPrefab;
        [SerializeField] protected SerializedDictionary<ItemCategory, Transform> itemInventory;
        [SerializeField] protected SerializedDictionary<ItemType, ItemDataSO> allDataSO;
        
        protected Dictionary<ItemDataSO, ItemData> _itemDatas = new Dictionary<ItemDataSO, ItemData>();
        protected Dictionary<ItemData, List<ItemCard>> _itemCards = new Dictionary<ItemData, List<ItemCard>>();
        
        //hold에 대해
        [SerializeField]private ItemHold realItem; //들리게 될 아이템(위치)

        #region EnDi
        private void OnEnable()
        {
            InGameItem.OnGetItem += AddItem;
            LoadCard.OnLoad += LoadItem;
            
            if(GameManager.Instance.isStart)
            {
                LoadItem();
            }
        }

        private void OnDisable()
        {
            InGameItem.OnGetItem -= AddItem;
            LoadCard.OnLoad -= LoadItem;
        }
        #endregion

        private void LoadItem() //불러오기
        {
            Dictionary<ItemType, List<int>> save = GameManager.Instance.PlayerStat.items.ToDictionary();

            foreach (KeyValuePair<ItemType, ItemDataSO> item in allDataSO.ToList())
            {
                ThrowItem(item.Value,9999999);
            }

            foreach (KeyValuePair<ItemType, List<int>> item in save.ToList())
            {
                foreach (int num in item.Value.ToList())
                {
                    AddItem(allDataSO[item.Key], num);
                }
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

        private void LessItem(ItemDataSO item, bool isThrow,int count = 1) //어쨌든 아이템 감소
        {
            if (_itemDatas.ContainsKey(item))
            {
                ItemData data = _itemDatas[item];
                
                data.UseItem(count, isThrow);
                _itemCards[data][_itemCards[data].Count -1].UpdateCountUI();
                
                realItem.CheckLessItem();
                
                switch(item.category) //개별 저장 애들은 걍 지워버리기
                {
                    case ItemCategory.food:
                        foreach (ItemCard card in _itemCards[data].ToList())
                        {
                            if(count != card.ReturnNum(true)) continue;
                            
                            _itemCards[data].Remove(card);
                            Destroy(card.gameObject);
                            break;
                        }
                        break;
                    case ItemCategory.armor:
                    case ItemCategory.weapon:
                    case ItemCategory.machine:
                        foreach (ItemCard card in _itemCards[data].ToList())
                        {
                            if(count != card.ReturnNum(false)) continue;
                            
                            _itemCards[data].Remove(card);
                            Destroy(card.gameObject);
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void NewCard(ItemDataSO item, bool isEtc, int star = 3, int hp = 100 )
        {
            //data 새 생성
            ItemData itemData = new ItemData();
            if (!isEtc)
            {
                itemData.NewItem(item);
                _itemDatas.Add(item, itemData);
            }
            else //기존거
            {
                itemData = _itemDatas[item];
            }
            
            Transform parent = itemInventory[item.category];
            
            //카드 새 생성
            ItemCard newCard = Instantiate(cardPrefab, parent);
            newCard.gameObject.SetActive(true);
            newCard.NewCard(itemData, star, hp);
            
            if(!isEtc)
                _itemCards.Add(itemData, new List<ItemCard>());

            _itemCards[itemData].Add(newCard);
        }

        public virtual void AddItem(ItemDataSO item, int count = 1)
        {
            switch(item.category)
            {
                case ItemCategory.seed:
                case ItemCategory.viand:
                case ItemCategory.stuff:
                case ItemCategory.special:
                    if (!_itemDatas.ContainsKey(item))
                        NewCard(item, false, 0,0);
                    break;

                case ItemCategory.food:
                case ItemCategory.armor:
                case ItemCategory.weapon:
                case ItemCategory.machine:
                    NewCard(item, _itemDatas.ContainsKey(item), count, count);
                    break;
            }

            ItemData data = _itemDatas[item];
            data.GetItem(count);
            
            ItemCard card = _itemCards[data][_itemCards[data].Count -1]; //갓 생성
            card.gameObject.SetActive(true);
            card.UpdateCountUI();
        }
    }
}