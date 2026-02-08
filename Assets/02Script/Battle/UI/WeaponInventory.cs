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
        [SerializeField] private BattleEntitySO inventoryCharacter;

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
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            WeaponInventoryCard.OnMouseClick -= CloseWeaponInventory;
            CollectItem.OnGetItem -= GetItem;
        }
        #endregion

        public void CloseWeaponInventory(WeaponInventoryCard _)
        {
            inventoryWindow.gameObject.SetActive(false);
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
            
            if (data.category == ItemCategory.weapon &&
                inventoryCharacter.useWeapons.Contains(_allWeaponDataSO[data.itemType]))
            {
                base.AddItem(data,count);
            }
        }
        #endregion

        #region Set
        protected override void SettingAllDataSO()
        {
            inventoryText.text = EnumToString.Name(inventoryCharacter.EntityName)+"의 무기 변경";
            
            _allWeaponDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                _allWeaponDataSO.Add(data.itemType, data as WeaponItemDataSO);
            }
        }
        public void SetInventoryCharacter(BattleEntitySO so) //누구의 인벤토리인지 지정해주기
        {
            inventoryCharacter = so;
            SettingAllDataSO();
        }
        #endregion
    }
}