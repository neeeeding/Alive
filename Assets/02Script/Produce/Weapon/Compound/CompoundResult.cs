using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.Produce.Weapon.Compound
{
    public class CompoundResult : MonoBehaviour
    {
        public static Action<ItemDataSO, WeaponArmorSaveData,int> OnGetItem;
        public static Action<ItemDataSO,int> OnUseItem;
        
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI buffT;
        [SerializeField] private TextMeshProUGUI checkT;
        [SerializeField] private TextMeshProUGUI resultT;
        [SerializeField] protected TextMeshProUGUI countUI;
        [SerializeField] protected Slider damageSlider;
        [SerializeField] private ProduceWindowActive window;
        private readonly int maxBuffCount = 5;
        
        private void OnEnable()
        {
            CompoundCheck.OnCompound += Compound;
        }

        private void OnDisable()
        {
            CompoundCheck.OnCompound -= Compound;
        }

        private void Compound(StuffItemDataSO stuff, WeaponArmorSaveData weapon, CompoundSelectWeaponArmorCard itemCard)
        {
            window.Check();
            
            List<BuffType> getBuffList = new List<BuffType>();

            foreach (BuffSO item in stuff.buffs)
            {
                if(!weapon.buffTypes.Contains(item.buffType))
                    getBuffList.Add(item.buffType);
            }

            foreach (WeaponArmorSaveData w in HouseManager.Instance.PlayerStat.weaponArmor[weapon.type].ToArray())
            {
                if(w != weapon) continue;
                HouseManager.Instance.PlayerStat.weaponArmor[weapon.type].Remove(w);
            }
            
            OnUseItem?.Invoke(stuff.baseData, 1);
            OnUseItem?.Invoke(itemCard.ReturnData().ReturnDataSO(), (int)weapon.hp);
            
            int check = Random.Range(0, 101); //성공 실패 유무
            
            resultT.text = "";
            if (check <= 30)
            {
                weapon.hp -= (weapon.hp / 100) * 5;
                
                resultT.text += "내구도가 5% 감소했습니다.\n";
            }
            else if (getBuffList.Count <= 0 || weapon.buffTypes.Count >= maxBuffCount) //내구도 증가
            {
                weapon.hp += 5;
                weapon.hp = Mathf.Min(weapon.hp, 100);
                
                resultT.text += "내구도가 5 증가했습니다.\n";
            }
            else
            {
                BuffType randBuff = getBuffList[Random.Range(0, getBuffList.Count)];
                weapon.buffTypes.Add(randBuff);
                if (stuff.weapon.itemType == weapon.type || stuff.armor.itemType == weapon.type)
                {
                    weapon.hp += 5;
                    weapon.hp = Mathf.Min(weapon.hp, 100);
                    
                    resultT.text += "내구도가 5 증가했습니다.\n";
                }
                resultT.text += $"{EnumToString.Name(randBuff)} 효과가 추가 되었습니다.";
                weapon.buffExplanation += $", [{EnumToString.Name(randBuff)}]";
            }
            
            HouseManager.Instance.PlayerStat.weaponArmor[weapon.type].Add(weapon);
            
            OnGetItem?.Invoke(itemCard.ReturnData().ReturnDataSO(),weapon,(int)weapon.hp);
            
            checkT.text = check <= 30? "합성에 실패하셨습니다." : "합성에 성공하셨습니다.";
            ShowWindow(weapon, itemCard.ReturnData().ReturnDataSO());
            UpdateCountUI(weapon);
        }

        private void ShowWindow(WeaponArmorSaveData weapon,ItemDataSO itemSo)
        {
            itemName.text = itemSo.itemName;
            itemImage.sprite = itemSo.itemImage;

            buffT.text = "";
            foreach (BuffType buff in weapon.buffTypes)
            {
                buffT.text += EnumToString.Name(buff) + "\n";
            }
        }
        private void UpdateCountUI(WeaponArmorSaveData data)
        {
            damageSlider.value = data.hp / 100;
            countUI.text = $"{data.hp} / 100";
        }
    }
}