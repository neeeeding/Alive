using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using _02Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon
{
    public class Result : MonoBehaviour
    {
        public static Action<ItemDataSO, WeaponArmorSaveData,int> OnGetItem;

        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI score;
        
        private void OnEnable()
        {
            GetItem();
        }
        
        private void GetItem()
        {
            if (SelectItemCard.curSelectItem == null) return;
            
            ItemDataSO item = SelectItemCard.curSelectItem.GetCurProduce(false);
            Setting(item);
            
            int hp = 100; //현재는 미니게임 안하니 내구도 100
            
            BuffSO buff = null;
            if (item is WeaponItemDataSO weapon)
            {
                buff = weapon.skillBuff;
            }
            else if (item is ArmorItemDataSO armor)
            {
                buff = armor.skillBuff;
            }
            
            WeaponArmorSaveData saveData = new WeaponArmorSaveData();
            saveData.buffTypes.Clear();
            saveData.buffTypes.Add(buff.buffType);
            saveData.type = item.itemType;
            saveData.hp = hp;
            if (!HouseManager.Instance.PlayerStat.weaponArmor.ToDictionary().ContainsKey(item.itemType))
            {
                HouseManager.Instance.PlayerStat.weaponArmor.Add(item.itemType, new List<WeaponArmorSaveData>());   
            }
            HouseManager.Instance.PlayerStat.weaponArmor[item.itemType].Add(saveData);
            
            OnGetItem?.Invoke(item,saveData, hp); //현재는 미니게임 안 하니 내구도 100
            SelectItemCard.curSelectItem = null;
        }

        private void Setting(ItemDataSO item)
        {
            itemImage.sprite = item.itemImage;
            itemName.text = item.itemName;
        }
    }
}