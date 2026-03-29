using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Produce.Weapon
{
    public class ProduceResult : MonoBehaviour
    {
        public static Action<ItemDataSO, WeaponArmorSaveData,int> OnGetItem;
        public static Action<ItemDataSO,int> OnUseItem;

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
            WeaponArmorSaveData saveData = new WeaponArmorSaveData();
            
            if (item is WeaponItemDataSO weapon)
            {
                buff = weapon.skillBuff;
                string front = weapon.itemExplanation.Split("스킬 ")[0];
                saveData.buffExplanation = front + "스킬 사용시 " + (buff.isDeBuff? "타겟에게" : "본인에게") 
                                                     + $" [{buff.buffName}]을/를 시전하고, ";
                saveData.explanation = $"타겟에게 데미지 {weapon.skillDamage}를 줍니다. (쿨타임 {weapon.collectTime}, 다수 타겟팅 "
                                       + (weapon.isGlobal? "가능" : "불가") + ")";
            }
            else if (item is ArmorItemDataSO armor)
            {
                buff = armor.skillBuff;
                string front = armor.itemExplanation.Split("사용시 ")[0];
                saveData.buffExplanation = front + $"사용시 받은 데미지를 {armor.damage} 감소 시키고, {armor.skillCoolTime}초 후에 " + (buff.isDeBuff? "타겟에게" : "본인에게") 
                                                     + $" [{buff.buffName}]을/를 시전합니다. (쿨타임 {armor.skillCoolTime})";
                saveData.explanation = "";
            }
            
            saveData.buffTypes.Clear();
            saveData.buffTypes.Add(buff.buffType);
            saveData.type = item.itemType;
            saveData.hp = hp;
            
            if (!HouseManager.Instance.PlayerStat.weaponArmor.ToDictionary().ContainsKey(item.itemType))
            {
                HouseManager.Instance.PlayerStat.weaponArmor.Add(item.itemType, new List<WeaponArmorSaveData>());   
            }
            HouseManager.Instance.PlayerStat.weaponArmor[item.itemType].Add(saveData);
            
            OnUseItem?.Invoke(SelectItemCard.curSelectItem.GetCurProduce(true),1);
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