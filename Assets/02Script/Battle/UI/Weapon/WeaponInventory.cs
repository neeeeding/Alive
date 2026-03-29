using System;
using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.Buff;
using _02Script.Battle.Entity;
using _02Script.Collect.Item;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Farming;
using _02Script.Inventory.Etc;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.Produce.Weapon;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Save;
using _02Script.UI.Store;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    public class WeaponInventory : LoadInventoryManager
    {
        public static Action<WeaponInventoryCard> OnDeleteWeapon;
        
        [Header("WeaponInventory")]
        [SerializeField] private BattleCharacter inventoryCharacter;

        [Header("Need")]
        [SerializeField] private GameObject inventoryWindow;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private BuffFind buffFind;
        
        private SerializedDictionary<ItemType, WeaponItemDataSO> _allWeaponDataSO = new SerializedDictionary<ItemType, WeaponItemDataSO>();
        private EntityName _weaponEntity;
        private bool _isBeforeAutoChage;

        #region EnDiAw
        protected override void OnEnable()
        {
            _isBeforeAutoChage = true;
            DialogItem.OnGetItem += GetOrThrowItem;
            WeaponArmorStartGiveItem.OnGetBuff += AddItem;
            InGameItem.OnGetItem += AddItem;
            Field.OnGetViand += AddItem;
            GameEvent.GameEvent.OnGetItem += AddItem;
            StoreCard.OnSellItem += AddItem;
            StoreCard.OnPayItem += ThrowItem;
            Field.OnUseSeed += ThrowItem;
            
            LoadCard.OnLoad += LoadItem;
            
            if(BattleSaveManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
            WeaponInventoryCard.OnMouseClick += CloseWeaponInventory;
            CollectItem.OnGetItem += GetItem;
            BattleCharacter.OnSkillWeapon += WeaponDamage;
            WeaponInventory.OnDeleteWeapon += DeleteWeapon;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            WeaponInventoryCard.OnMouseClick -= CloseWeaponInventory;
            WeaponArmorStartGiveItem.OnGetBuff -= AddItem;
            CollectItem.OnGetItem -= GetItem;
            BattleCharacter.OnSkillWeapon -= WeaponDamage;
            WeaponInventory.OnDeleteWeapon -= DeleteWeapon;
        }
        #endregion

        private void CloseWeaponInventory(WeaponInventoryCard _w,List<BuffSO> _b, EntityName _e)
        {
            inventoryWindow.gameObject.SetActive(false);
        }
        protected override void LoadItem() //불러오기
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = BattleSaveManager.Instance.PlayerStat.items.ToDictionary();
            Dictionary<ItemType, List<WeaponArmorSaveData>> etcData = BattleSaveManager.Instance.PlayerStat.weaponArmor.ToDictionary();
            LoadItem(save, etcData);
        }

        #region WeaponDamage
        //데미지 감소
        private void WeaponDamage(WeaponInventoryCard weapon, float minus)
        {
            if(!weapon) return;
            if (!ItemDatas.ContainsKey(weapon.ReturnData().ReturnDataSO())) return;
            ItemData data = ItemDatas[weapon.ReturnData().ReturnDataSO()];
            
            WeaponInventoryCard useCard = weapon;
            foreach (ItemCard d in ItemCards[data]) //다른 인벤토리에서는 '카드'자체의 인스턴스 값이 달라 적용이 안되므로
            {
                if(d.ReturnNum(false) != weapon.ReturnNum(false)) continue;
                useCard = d as WeaponInventoryCard;
                break;
            }

            //data.UseItem(weapon.ReturnNum(false),minus, true); //데미지 삭제 (해봤자 아래 때문에 데미지 두 번 삭제됨)
            useCard.ItemDamage(minus);
            useCard.UpdateCountUI();
                
            if(useCard.ReturnNum(false) > 0) return; //내구도 0 이하시 삭제 시작
            WarringManager.Warring.ShowWarring(
                $"{EnumToString.Name(useCard.ReturnData().ReturnDataSO().itemType)}의 내구력이 다하여 부셔졌습니다.");
            
            OnDeleteWeapon?.Invoke(useCard);
            AutoWeaponSelect();
        }

        private void DeleteWeapon(WeaponInventoryCard weapon) //무기 정보 소멸 및 삭제
        {
            ItemData data = weapon.ReturnData();
            GameObject soonDelete = null;
            
            if(!ItemDatas.ContainsKey(data.ReturnDataSO())) return;
            
            foreach (ItemCard w in ItemCards[ItemDatas[data.ReturnDataSO()]])
            {
                if(w != weapon) continue;
                soonDelete = w.gameObject;
                break;
            }
            
            ItemCards[ItemDatas[data.ReturnDataSO()]].Remove(weapon);
            if (ItemCards[ItemDatas[data.ReturnDataSO()]].Count <= 0) //무기 다 사라지면 없애버리기
                ItemDatas.Remove(data.ReturnDataSO());
            
            Destroy(soonDelete);
        }
        #endregion

        #region GetAddItem (inventory)
        private void GetItem(ItemDataSO data, int count, EntityName type) //아이템 얻은거, 카드도 생성
        {
            WeaponArmorSaveData save = null;
            foreach (WeaponArmorSaveData saveData in BattleSaveManager.Instance.PlayerStat.weaponArmor[data.itemType].ToArray())
            {
                if (saveData.hp == count)
                {
                    save = saveData;
                    BattleSaveManager.Instance.PlayerStat.weaponArmor[data.itemType].Remove(saveData);//hp가 겹칠 때 계속 해당 데이터만 참고하게 될테니, 또한 전투하면서 hp 계속 변경 될거니까
                }
            }

            AddItem(data,save,count);
        }
        public override void AddItem(ItemDataSO item,WeaponArmorSaveData saveData, int count = 1)
        {
            AddItem(item, count);
        }
        public override void AddItem(ItemDataSO item, int count = 1)
        {
            if(item.category != ItemCategory.weapon) return;
            
            if(_allWeaponDataSO.Count <= 0)
                SettingAllDataSO();
            
            if (_allWeaponDataSO.ContainsKey(item.itemType))
            {
                base.AddItem(_allWeaponDataSO[item.itemType], count);
                
                ItemData d = ItemDatas[_allWeaponDataSO[item.itemType]]; //그냥 item하면 인스턴스 값이 달라서 못함.
                (ItemCards[d][ItemCards[d].Count -1] as WeaponInventoryCard).Set(_weaponEntity);
                
                if (item is WeaponItemDataSO)
                {
                    WeaponInventoryCard card = ItemCards[d][ItemCards[d].Count -1] as WeaponInventoryCard;
                    card.NewCard(buffFind,d,0,count);
                }
            }
            if (_isBeforeAutoChage)
            {
                AutoWeaponSelect();
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
                if (inventoryCharacter.ReturnSO().useWeapons.Contains(data as WeaponItemDataSO))
                    _allWeaponDataSO.Add(data.itemType, data as WeaponItemDataSO);
            }
        }
        public void SetInventoryCharacter(BattleCharacter character) //누구의 인벤토리인지 지정해주기
        {
            inventoryCharacter = character;
            _weaponEntity = character.ReturnName();
            SettingAllDataSO();
            AutoWeaponSelect();
        }

        private void AutoWeaponSelect()
        {
            if (ItemCards.Count <= 0)
            {
                _isBeforeAutoChage = true;
                inventoryCharacter.ChangeWeapon(null,null,_weaponEntity);
                return;
            }
            _isBeforeAutoChage = false;
            WeaponInventoryCard weapon = ItemCards.First().Value[0] as WeaponInventoryCard;
            
            List<BuffSO> buff = new List<BuffSO>();
            
            Dictionary<ItemType, List<WeaponArmorSaveData>> dict = BattleSaveManager.Instance?.PlayerStat?.weaponArmor?.ToDictionary();

            if (weapon == null ||dict == null ||
                dict.Count <= 0 ||
                !dict.ContainsKey(weapon.ReturnData().ReturnDataSO().itemType) ||
                dict[weapon.ReturnData().ReturnDataSO().itemType] == null ||
                dict[weapon.ReturnData().ReturnDataSO().itemType].Count <= 0 ||
                dict[weapon.ReturnData().ReturnDataSO().itemType][0] == null ||
                dict[weapon.ReturnData().ReturnDataSO().itemType][0].buffTypes == null)return;

            foreach (BuffType b in dict[weapon.ReturnData().ReturnDataSO().itemType][0].buffTypes)
            {
                buff.Add(buffFind.GetBuff(b));
            }
            
            inventoryCharacter.ChangeWeapon(weapon,buff,_weaponEntity);
        }
        #endregion
    }
}