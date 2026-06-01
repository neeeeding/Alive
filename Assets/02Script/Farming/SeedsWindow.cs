using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _02Script.InGameDebug;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Produce.Weapon;
using UnityEngine;

namespace _02Script.Farming
{
    public class SeedsWindow : LoadInventoryManager
    {
        [Space(50)]
        [Header("Seeds")]
        [SerializeField] protected SeedsSO[] _allSO;
        [SerializeField] private GameObject window;
        [SerializeField] private Field field;

        protected Dictionary<ItemType, SeedsSO> _allDataSO;
        
        public void CloseBtn(SeedsSO _)
        {
            CloseBtn();
        }
        public void CloseBtn()
        {
            window.SetActive(false);
        }
        protected override void LoadItem()
        {
            SettingAllDataSO();
    
            foreach (var cardList in ItemCards.Values)
            foreach (var card in cardList)
                if (card != null) Destroy(card.gameObject);
            
            ItemCards.Clear();
            ItemDatas.Clear();

            Dictionary<ItemType, List<float>> save = HouseManager.Instance.PlayerStat.items.ToDictionary();

            foreach (KeyValuePair<ItemType, List<float>> item in save.ToList())
            {
                if (!_allDataSO.ContainsKey(item.Key)) continue;

                float val = item.Value[0]; 
                int count = (int)val;

                if (count <= 0) continue;

                SeedsSO seedsSO = _allDataSO[item.Key];
                NewCard(seedsSO.seeds, false, 0, 0);

                if (ItemDatas.ContainsKey(seedsSO.seeds))
                {
                    ItemData data = ItemDatas[seedsSO.seeds];
                    data.SetCountOnly(count);
                    ItemCards[data][ItemCards[data].Count - 1].UpdateCountUI();
                }
            }
            field.LoadFarm(_allDataSO);
        }

        protected override void NewCard(ItemDataSO item, bool isEtc, int star = 3, int hp = 100,WeaponArmorSaveData saveData = null )
        {
            if(_allDataSO == null || !_allDataSO.ContainsKey(item.itemType)) return;
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
            SeedsCard newCard = Instantiate(cardPrefab as SeedsCard, parent);
            newCard.gameObject.SetActive(true);
            newCard.NewCard(_allDataSO[item.itemType],itemData);
            
            if(!isEtc)
                ItemCards.Add(itemData, new List<ItemCard>());

            ItemCards[itemData].Add(newCard);
        }

        protected override void SettingAllDataSO()
        {
            _allDataSO = new Dictionary<ItemType, SeedsSO>();

            foreach (SeedsSO data in _allSO)
            {
                _allDataSO.Add(data.seeds.itemType, data);
            }
        }


        protected override void LessItem(ItemDataSO item, bool isThrow, int count = 1, WeaponArmorSaveData saveData = null) //어쨌든 아이템 감소
        {
            if (_allDataSO == null || !_allDataSO.ContainsKey(item.itemType) || !ItemDatas.ContainsKey(_allDataSO[item.itemType].seeds)) return;
            
            ItemData data = ItemDatas[_allDataSO[item.itemType].seeds];
                
            data.UseItem(count, isThrow);
            ItemCards[data][ItemCards[data].Count -1].UpdateCountUI();
        }
    }
}