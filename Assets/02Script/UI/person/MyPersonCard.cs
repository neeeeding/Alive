using System;
using _02Script.Etc;
using _02Script.Obj.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.UI.person
{
    public class MyPersonCard : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TextMeshProUGUI hptext;

        private void OnEnable()
        {
            Setting();
        }

        private void Setting()
        {
            float hp = StatCalculate.Calculate(EntityName.lie, StatsType.HpStat);
            float maxHp = StatCalculate.Calculate(EntityName.lie, StatsType.HpStat);
            hpSlider.value = (int)hp/maxHp;
            hptext.text = $"{(int)hp} / {maxHp}";
        }
    }
    
    [Flags]
    public enum StatsType
    {
        //450, 1 == 10
        none = 0,
        curHp = 1 << 0, //캐릭터의 현재 체력
        attack = 1 << 1, //타격
        agility = 1 << 2, //민첩
        defense = 1 << 3, //방어
        skill = 1 << 4, //숙련
        recovery = 1 << 5, //회복
        tolerance = 1 << 6, //내성
        duration = 1 << 7, //지속
        acceptance = 1 << 8, //수납
        mining = 1 << 9, //채굴
        HpStat = 1 << 10, //체력 (등급)
    }
}