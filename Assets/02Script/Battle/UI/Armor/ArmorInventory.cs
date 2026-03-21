using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Battle.Entity;
using _02Script.Battle.UI.Weapon;
using _02Script.Collect.Item;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Farming;
using _02Script.Inventory.Inventory;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.UI.Dialog.Dialog;
using _02Script.UI.Save;
using _02Script.UI.Store;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace _02Script.Battle.UI.Armor
{
    public class ArmorInventory : LoadInventoryManager
    {
        public static Action<ArmorInventoryCard> OnDeleteArmor;
        
        [Header("ArmorInventory")]
        [SerializeField] private BattleCharacter inventoryCharacter;

        [Header("Need")]
        [SerializeField] private GameObject inventoryWindow;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private BuffFind buffFind;
        
        private SerializedDictionary<ItemType, ArmorItemDataSO> _allArmorDataSO = new SerializedDictionary<ItemType, ArmorItemDataSO>();
        private EntityName _armorEntity;

        #region EnDiAw
        protected override void OnEnable()
        {
            DialogItem.OnGetItem += GetOrThrowItem;
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
            ArmorInventoryCard.OnMouseClick += CloseWeaponInventory;
            CollectItem.OnGetItem += GetItem;
            BattleCharacter.OnUseArmor += ArmorDamage;
            ArmorInventory.OnDeleteArmor += DeleteArmor;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ArmorInventoryCard.OnMouseClick -= CloseWeaponInventory;
            CollectItem.OnGetItem -= GetItem;
            BattleCharacter.OnUseArmor -= ArmorDamage;
            ArmorInventory.OnDeleteArmor -= DeleteArmor;
        }
        #endregion

        private void CloseWeaponInventory(ArmorInventoryCard _a,List<BuffSO> _b, EntityName _e)
        {
            inventoryWindow.gameObject.SetActive(false);
        }
        protected override void LoadItem() //불러오기
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = BattleSaveManager.Instance.PlayerStat.items.ToDictionary();
            LoadItem(save);
        }

        #region WeaponDamage
        //데미지 감소
        private void ArmorDamage(ArmorInventoryCard armor)
        {
            if(!armor) return;
            if (!ItemDatas.ContainsKey(armor.ReturnData().ReturnDataSO())) return;
            ItemData data = ItemDatas[armor.ReturnData().ReturnDataSO()];
            
            ArmorInventoryCard useCard = armor;
            foreach (ItemCard d in ItemCards[data]) //다른 인벤토리에서는 '카드'자체의 인스턴스 값이 달라 적용이 안되므로
            {
                if(d.ReturnNum(false) != armor.ReturnNum(false)) continue;
                useCard = d as ArmorInventoryCard;
                break;
            }
            
            useCard.ItemDamage((armor.ReturnData().ReturnDataSO() as ArmorItemDataSO).damage);
            useCard.UpdateCountUI();
                
            if(useCard.ReturnNum(false) > 0) return; //내구도 0 이하시 삭제 시작
            WarringManager.Warring.ShowWarring(
                $"{EnumToString.Name(useCard.ReturnData().ReturnDataSO().itemType)}의 내구력이 다하여 부셔졌습니다.");
            
            OnDeleteArmor?.Invoke(useCard);
        }

        private void DeleteArmor(ArmorInventoryCard armor) //갑옷 정보 소멸 및 삭제
        {
            ItemData data = armor.ReturnData();
            GameObject soonDelete = null;
            
            if(!ItemDatas.ContainsKey(data.ReturnDataSO())) return;
            
            foreach (ItemCard w in ItemCards[ItemDatas[data.ReturnDataSO()]])
            {
                if(w != armor) continue;
                soonDelete = w.gameObject;
                break;
            }
            
            ItemCards[ItemDatas[data.ReturnDataSO()]].Remove(armor);
            if (ItemCards[ItemDatas[data.ReturnDataSO()]].Count <= 0) //갑옷 다 사라지면 없애버리기
                ItemDatas.Remove(data.ReturnDataSO());
            
            Destroy(soonDelete);
        }
        #endregion

        #region GetAddItem (inventory)
        private void GetItem(ItemDataSO data, int count, EntityName type) //아이템 얻은거, 카드도 생성
        {
            AddItem(data,count);
        }
        public override void AddItem(ItemDataSO item, int count = 1)
        {
            if(_allArmorDataSO.Count <= 0)
                SettingAllDataSO();
            
            if (_allArmorDataSO.ContainsKey(item.itemType))
            {
                base.AddItem(_allArmorDataSO[item.itemType], count);
                ItemData d = ItemDatas[_allArmorDataSO[item.itemType]]; //그냥 item하면 인스턴스 값이 달라서 못함.
                (ItemCards[d][ItemCards[d].Count -1] as ArmorInventoryCard).Set(_armorEntity);
                
                if (item is ArmorItemDataSO)
                {
                    ArmorInventoryCard card = ItemCards[d][ItemCards[d].Count -1] as ArmorInventoryCard;
                    card.NewCard(buffFind,d,0,count);
                }
            }
        }
        #endregion

        #region Set
        protected override void SettingAllDataSO()
        {
            if(inventoryCharacter == null) return;
            
            inventoryText.text = EnumToString.Name(inventoryCharacter.ReturnSO().EntityName)+"의 갑옷 변경";
            
            _allArmorDataSO.Clear();

            foreach (ItemDataSO data in allSO)
            {
                _allArmorDataSO.Add(data.itemType, data as ArmorItemDataSO);
            }
        }
        public void SetInventoryCharacter(BattleCharacter character) //누구의 인벤토리인지 지정해주기
        {
            inventoryCharacter = character;
            _armorEntity = character.ReturnName();
            SettingAllDataSO();
        }
        #endregion
    }
}