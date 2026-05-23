using System;
using _02Script.Battle.Food;
using _02Script.DoTweenUI.Warring;
using _02Script.Etc;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.Inventory.Inventory.Use
{
    public class UseWindow  : MonoBehaviour
    {
        public static Action<ItemData, int> OnHold;
        public static Action<ItemDataSO, int> OnUse;
        public static Action<ItemDataSO, int> OnThrow;
        public static Action<EntityName, StatsType, int> OnGetStat;

        [SerializeField] private GameObject foodPerson;
        [SerializeField] private TMP_InputField countInputField;
        [SerializeField] private Slider countSlider;

        private ItemCard card;
        private int maxNum;
        private float useNum;
        private EntityName entityName;
        
        
        private void OnEnable()
        {
            entityName = EntityName.None;
            FoodPersonCard.OnPerson += ChangeColor;
        }

        private void OnDisable()
        {
            FoodPersonCard.OnPerson -= ChangeColor;
            foodPerson.SetActive(false);
        }

        private void ChangeColor(EntityName obj)
        {
            entityName = obj;
        }

        public void SetData(ItemCard data, float selfCheck)
        {
            card = data;
            ItemData so = data.ReturnData();
            
            ItemCategory category = data.ReturnData().ReturnDataSO().category;

            if (category != ItemCategory.food && category != ItemCategory.weapon &&
                category != ItemCategory.armor && category != ItemCategory.machine)
            {
                maxNum = so.ItemCount();
                useNum = -1;
            }
            else
            {
                if (category == ItemCategory.food)
                {
                    foodPerson.SetActive(true);
                }
                maxNum = 1;
                useNum = selfCheck;
            }
            countSlider.value = (float)1/maxNum;
            countInputField.text = 1.ToString();
        }
        
        public void SliderMove()
        {
            float x = countSlider.value;
            x *= maxNum;
            countInputField.text = ((int)x).ToString();
        }

        public void InputFieldInput()
        {
            int x = int.Parse(countInputField.text);
            countSlider.value = (float)x/maxNum;
        }

        public void HoldData()
        {
            OnHold?.Invoke(card.ReturnData(), (int)(useNum >= 0 ? useNum : 
                int.Parse(countInputField.text)));
            
            gameObject.SetActive(false);
        }

        public void UseData()
        {
            
            int rand = 1;
            if (card.ReturnData().ReturnDataSO().category == ItemCategory.food ||
                card.ReturnData().ReturnDataSO().category == ItemCategory.seed)
            {
                if (entityName == EntityName.None)
                {
                    WarringManager.Warring.ShowWarring("섭취할 캐릭터를 정해주세요.");
                    return;
                }
                rand = Random.Range(1,6 -(int)card.ReturnNum(true));
            }

            if(rand == 1)
            {
                StatsType stat = card.ReturnData().ReturnDataSO().stats;
                int add = card.ReturnData().ReturnDataSO().addStats;
                OnGetStat?.Invoke(entityName, stat ,add);
                

                string warring = $"섭취에 성공하셨습니다!\n{EnumToString.Name(entityName)}의 {EnumToString.Name(stat)}이/가 ";
                if (stat == StatsType.curHp)
                {
                    warring += $"{add} ";
                    warring += add >= 0 ? "만큼 회복합니다." : "만큼 감소합니다.";
                }
                else
                {
                    warring += $"{StatCalculate.StatAlphabet(entityName, stat)}";
                    warring += add >= 0 ? " 로 향상됩니다." : " 로 하락합니다.";
                }
                
                WarringManager.Warring.ShowWarring(warring,3);
                
                OnUse?.Invoke(card.ReturnData().ReturnDataSO(), (int)(useNum >= 0 ? useNum : 
                    int.Parse(countInputField.text)));
            }
            else
            {
                WarringManager.Warring.ShowWarring("섭취에 실패하셨습니다...");
                OnThrow?.Invoke(card.ReturnData().ReturnDataSO(), (int)(useNum >= 0 ? useNum : 
                    int.Parse(countInputField.text)));
            }
            
            gameObject.SetActive(false);
        }
        
        public void ThrowData()
        {
            OnThrow?.Invoke(card.ReturnData().ReturnDataSO(), (int)(useNum >= 0 ? useNum : 
                int.Parse(countInputField.text)));
            
            gameObject.SetActive(false);
        }
    }
}