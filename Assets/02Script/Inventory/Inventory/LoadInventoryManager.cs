using System.Collections.Generic;
using System.Linq;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using _02Script.Manager;
using _02Script.Produce.Weapon;
using _02Script.SaveData;
using _02Script.UI.Save;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02Script.Inventory.Inventory
{
    public class LoadInventoryManager : InventoryManager
    {
        [SerializeField] protected ItemDataSO[] allSO;
        
        protected SerializedDictionary<ItemType, ItemDataSO> AllDataSO;

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadCard.OnLoad += LoadItem;
            HouseManager.OnStart += LoadItem;
    
            if(HouseManager.Instance.isStart && ItemDatas.Count <= 0)
            {
                LoadItem();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LoadCard.OnLoad -= LoadItem;
            HouseManager.OnStart -= LoadItem;
        }

        protected virtual void LoadItem() //불러오기
        {
            SettingAllDataSO();
            Dictionary<ItemType, List<float>> save = HouseManager.Instance.PlayerStat.items.ToDictionary();
            Dictionary<ItemType, List<WeaponArmorSaveData>> etcData = HouseManager.Instance.PlayerStat.weaponArmor.ToDictionary();
            LoadItem(save, etcData);
        }

        protected virtual void LoadItem(Dictionary<ItemType, List<float>> save, Dictionary<ItemType, List<WeaponArmorSaveData>> etcData)
        {
            foreach (var cardList in ItemCards.Values)
            foreach (var card in cardList)
                if (card != null) Destroy(card.gameObject);
            ItemCards.Clear();
            ItemDatas.Clear();

            foreach (KeyValuePair<ItemType, List<float>> item in save.ToList())
            {
                if (item.Key == ItemType.notting) continue;
                if (AllDataSO == null)
                {
                    SettingAllDataSO();
                }
                
                if (!AllDataSO.ContainsKey(item.Key)) continue;

                ItemDataSO so = AllDataSO[item.Key];

                LoadItem(item, etcData, so);
            }
        }
        protected virtual void LoadItem(KeyValuePair<ItemType, List<float>> item, Dictionary<ItemType, List<WeaponArmorSaveData>> etcData,ItemDataSO so)
        {
            switch (so.category)
            {
                case ItemCategory.food:
                case ItemCategory.weapon:
                case ItemCategory.armor:
                case ItemCategory.machine:
                    int count = item.Value.Count;
                    for (int i = 1; i < count; i++)
                    {
                        WeaponArmorSaveData saveData = null;
                        if (etcData.ContainsKey(item.Key) && etcData[item.Key].Count >= i)
                            saveData = etcData[item.Key][i - 1];
                        else
                            saveData = NewSaveData(so, (int)item.Value[i]);
                            
                        NewCard(so, ItemDatas.ContainsKey(so), (int)item.Value[i], (int)item.Value[i],saveData);
                        if (!ItemDatas.ContainsKey(so))
                        {
                            continue;
                        }
                        ItemData data = ItemDatas[so];
                        data.AddCountOnly();
                        ItemCards[data][ItemCards[data].Count - 1].UpdateCountUI();
                    }
                    break;

                default:
                {
                    float val = item.Value[0];
                        
                    NewCard(so, false, 0, 0);
                    if (ItemDatas.ContainsKey(so))
                    {
                        ItemData data = ItemDatas[so];
                        data.SetCountOnly((int)val);
                        ItemCards[data][ItemCards[data].Count - 1].UpdateCountUI();
                    }
                }
                    break;
            }
        }

        protected override void NewCard(ItemDataSO item, bool isEtc, int star = 3, int hp = 100, WeaponArmorSaveData saveData = null)
        {
            if(AllDataSO != null&&AllDataSO.ContainsKey(item.itemType))
                item = AllDataSO[item.itemType];
            base.NewCard(item, isEtc, star, hp, saveData);
        }

        protected override void LessItem(ItemDataSO item, bool isThrow, int count = 1, WeaponArmorSaveData saveData = null) //어쨌든 아이템 감소
        {
            if (AllDataSO == null || !AllDataSO.ContainsKey(item.itemType)) return;
            
            item = AllDataSO[item.itemType];
            base.LessItem(item, isThrow, count, saveData);
        }

        protected virtual void SettingAllDataSO()
        {
            AllDataSO = new SerializedDictionary<ItemType, ItemDataSO>();

            foreach (ItemDataSO data in allSO)
            {
                AllDataSO.Add(data.itemType, data);
            }
        }

        protected WeaponArmorSaveData NewSaveData(ItemDataSO item, int hp) // data 없는 애들
        {                
            WeaponArmorSaveData data = new WeaponArmorSaveData();
            BuffSO buff = null;
            if (item is WeaponItemDataSO weapon)
            {
                buff = weapon.skillBuff;
                string front = weapon.itemExplanation.Split("스킬 ")[0];
                if (buff == null)
                {
                    data.buffExplanation = front + "스킬 사용시 ";
                }
                else
                {
                    data.buffExplanation = front + "스킬 사용시 " + (buff.isDeBuff? "타겟에게" : "본인에게") 
                                           + $" [{buff.buffName}]을/를 시전하고, ";
                }
                data.explanation = $"타겟에게 데미지 {weapon.skillDamage}를 줍니다. (쿨타임 {weapon.collectTime}, 다수 타겟팅 "
                                   + (weapon.isGlobal? "가능" : "불가") + ")";
            }
            else if (item is ArmorItemDataSO armor)
            {
                buff = armor.skillBuff;
                string front = armor.itemExplanation.Split("사용시 ")[0];
                if (buff == null)
                {
                    data.buffExplanation = front + $"사용시 받은 데미지를 {armor.damage} 감소 시킵니다.";
                }
                else
                {
                    data.buffExplanation = front + $"사용시 받은 데미지를 {armor.damage} 감소 시키고, {armor.skillCoolTime}초 후에 " + (buff.isDeBuff? "타겟에게" : "본인에게") 
                                           + $" [{buff.buffName}]을/를 시전합니다. (쿨타임 {armor.skillCoolTime})";
                }
                data.explanation = "";
            }
            
            data.buffTypes.Clear();
            if(buff != null)
                data.buffTypes.Add(buff.buffType);
            data.type = item.itemType;
            data.hp = hp;

            return data;
        }
    }
}