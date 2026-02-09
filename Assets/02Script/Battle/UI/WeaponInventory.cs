using System.Linq;
using _02Script.Battle.Entity;
using _02Script.Collect.Item;
using _02Script.Etc;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.UI
{
    public class WeaponInventory : LoadInventoryManager
    {
        [Header("WeaponInventory")]
        [SerializeField] private BattleCharacter inventoryCharacter;

        [Header("Need")]
        [SerializeField] private GameObject inventoryWindow;
        [SerializeField] private TextMeshProUGUI inventoryText;
        
        private SerializedDictionary<ItemType, WeaponItemDataSO> _allWeaponDataSO = new SerializedDictionary<ItemType, WeaponItemDataSO>();

        #region EnDiAw
        protected override void OnEnable()
        {
            base.OnEnable();
            WeaponInventoryCard.OnMouseClick += CloseWeaponInventory;
            CollectItem.OnGetItem += GetItem;
            BattleCharacter.OnSkillWeapon += WeaponDamage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            WeaponInventoryCard.OnMouseClick -= CloseWeaponInventory;
            CollectItem.OnGetItem -= GetItem;
            BattleCharacter.OnSkillWeapon -= WeaponDamage;
        }
        #endregion

        private void CloseWeaponInventory(WeaponInventoryCard _)
        {
            inventoryWindow.gameObject.SetActive(false);
        }

        //데미지 감소
        private void WeaponDamage(WeaponInventoryCard weapon, float minus)
        {
            if (!ItemDatas.ContainsKey(weapon.ReturnData().ReturnDataSO())) return;
            ItemData data = ItemDatas[weapon.ReturnData().ReturnDataSO()];
                
            data.UseItem(weapon.ReturnNum(false),minus, true); //데미지 삭제
            
            foreach (ItemCard card in ItemCards[data].ToList()) //체력바 업데이트 하는 겸, 0 미만 삭제
            {
                card.UpdateCountUI();
                
                if(0 >= card.ReturnNum(false)) continue;
                            
                ItemCards[data].Remove(card);
                if(ItemCards[data].Count <= 0) //무기 다 사라지면 없애버리기
                    ItemDatas.Remove(card.ReturnData().ReturnDataSO());
                Destroy(card.gameObject);
                break;
            }
        }

        #region GetAddItem (inventory)
        private void GetItem(ItemDataSO data, int count, EntityName type) //아이템 얻은거, 카드도 생성
        {
            AddItem(data,count);
        }
        public override void AddItem(ItemDataSO data, int count = 1)
        {
            if(_allWeaponDataSO.Count <= 0)
                SettingAllDataSO();
            
            if (_allWeaponDataSO.ContainsKey(data.itemType))
            {
                base.AddItem(_allWeaponDataSO[data.itemType], count);
            }
        }
        #endregion

        #region Set
        protected override void SettingAllDataSO()
        {
            if(inventoryCharacter == null) return;
            
            inventoryText.text = EnumToString.Name(inventoryCharacter.ReturnSO().EntityName)+"의 무기 변경";
            
            _allWeaponDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                if(inventoryCharacter.ReturnSO().useWeapons.Contains(data as WeaponItemDataSO))
                    _allWeaponDataSO.Add(data.itemType, data as WeaponItemDataSO);
            }
        }
        public void SetInventoryCharacter(BattleCharacter character) //누구의 인벤토리인지 지정해주기
        {
            inventoryCharacter = character;
            SettingAllDataSO();
            AutoWeaponSelect();
        }

        private void AutoWeaponSelect()
        {
            if (ItemCards.Count <= 0)
            {
                inventoryCharacter.ChangeWeapon(null);
                return;
            }
            inventoryCharacter.ChangeWeapon(ItemCards.First().Value[0] as WeaponInventoryCard);
        }
        #endregion
    }
}