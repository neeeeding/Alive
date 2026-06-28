using System.Linq;
using _02Script.Battle.Buff;
using _02Script.Etc;
using _02Script.Produce;
using _02Script.Produce.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Inventory.Item
{
    public class ItemCard : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] protected TextMeshProUGUI countUI;
        [SerializeField] protected Image cardImage;
        
        protected ItemData itemData;
        
        protected float star; // 별 등급 (1 ~ 5)
        protected float itemHp; // 내구도 (0 ~ 100)
        protected WeaponArmorSaveData weaponArmorBuff;

        public ItemData ReturnData()
        {
            return itemData;
        }

        public float ReturnNum(bool isStar)
        {
            return isStar ? star : itemHp;
        }

        public WeaponArmorSaveData ReturnSaveData()
        {
            if (weaponArmorBuff != null)
                return weaponArmorBuff;
            return NewSaveData(weaponArmorBuff);
        }
        
        // [수정] 무기/방어구 사용 시 내구도 감소 및 세이브 데이터(PlayerStat) 실시간 완벽 동기화
        public void ItemDamage(float damage)
        {
            float oldHp = itemHp;
            itemHp = Mathf.Max(0, itemHp - damage);
            if (weaponArmorBuff != null)
            {
                weaponArmorBuff.hp = itemHp;
            }

            if (itemData != null && itemData.ReturnDataSO() != null)
            {
                ItemType type = itemData.ReturnDataSO().itemType;
                var stat = SaveManagerCheck.GetCurScenePlayerStat();
                if (stat != null)
                {
                    // 1. items float 리스트 동기화
                    if (stat.items != null && stat.items.ContainsKey(type))
                    {
                        var list = stat.items[type];
                        int idx = -1;
                        for (int i = 1; i < list.Count; i++)
                        {
                            if (Mathf.Abs(list[i] - oldHp) < 0.1f)
                            {
                                idx = i;
                                break;
                            }
                        }
                        if (idx > 0)
                        {
                            list[idx] = itemHp;
                        }
                    }

                    // 2. weaponArmor 상세 데이터 리스트 동기화
                    if (stat.weaponArmor != null && stat.weaponArmor.ContainsKey(type))
                    {
                        var waList = stat.weaponArmor[type];
                        if (weaponArmorBuff != null && waList.Contains(weaponArmorBuff))
                        {
                            weaponArmorBuff.hp = itemHp;
                        }
                        else
                        {
                            var target = waList.FirstOrDefault(w => w != null && Mathf.Abs(w.hp - oldHp) < 0.1f);
                            if (target != null)
                            {
                                target.hp = itemHp;
                            }
                        }
                    }
                }
            }
        }

        public virtual void NewCard(ItemData itemData, int setStar = 0, int setItemHp = 100, WeaponArmorSaveData saveData = null)
        {
            this.itemData = itemData;
            cardImage.sprite = this.itemData.ReturnDataSO().itemImage;
            
            if ((saveData == null || saveData.explanation == null) && 
                (itemData.ReturnDataSO().category == ItemCategory.weapon || itemData.ReturnDataSO().category == ItemCategory.armor))
                saveData = NewSaveData(saveData);
            weaponArmorBuff = saveData;
            star = setStar;
            itemHp = setItemHp;
        }

        protected virtual void OnEnable()
        {
            UpdateCountUI();
        }

        public virtual void UpdateCountUI()
        {
            if (itemData == null) return;
            
            int count = itemData.ItemCount();
            
            countUI.text = count.ToString();

            ItemCategory category = itemData.ReturnDataSO().category;

            if (category != ItemCategory.seed && category != ItemCategory.special &&
                category != ItemCategory.viand && category != ItemCategory.stuff)
            {
                countUI.text = "";
            }
            
            if (count <= 0 && !(itemData.ReturnDataSO() is ProduceBookSO))
            {
                gameObject.SetActive(false);
            }
        }

        protected WeaponArmorSaveData NewSaveData(WeaponArmorSaveData data)
        {                
            data = new WeaponArmorSaveData();
            ItemDataSO item = ReturnData().ReturnDataSO();
            BuffSO buff = null;
            if (item is WeaponItemDataSO weapon)
            {
                buff = weapon.skillBuff;
                
                string front = weapon.itemExplanation.Split("스킬 ")[0];
                data.buffExplanation = front;

                if (buff != null)
                {
                    data.buffExplanation += "스킬 사용시 " + (buff.isDeBuff ? "타겟에게" : "본인에게") 
                                                      + $" [{buff.buffName}]을/를 시전하고, ";
                    data.explanation = $"타겟에게 데미지 {weapon.skillDamage}를 줍니다. (쿨타임 {weapon.collectTime}, 다수 타겟팅 "
                                       + (weapon.isGlobal ? "가능" : "불가") + ")";
                }
            }
            else if (item is ArmorItemDataSO armor)
            {
                buff = armor.skillBuff;
                string front = armor.itemExplanation.Split("사용시 ")[0];
                
                data.buffExplanation = front;

                if (buff != null)
                {
                    data.buffExplanation += $"사용시 받은 데미지를 {armor.damage} 감소 시키고, {armor.skillCoolTime}초 후에 " +
                                           (buff.isDeBuff ? "타겟에게" : "본인에게")
                                           + $" [{buff.buffName}]을/를 시전합니다. (쿨타임 {armor.skillCoolTime})";
                }
                data.explanation = "";
            }
            else
            {
                return data;
            }

            if (buff != null)
            {
                data.buffTypes.Add(buff.buffType);
            }
            data.type = item.itemType;
            data.hp = ReturnNum(false);

            return data;
        }
    }
}
