using System;
using System.ComponentModel;
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
        [Description("없음")]none = 0,
        [Description("HP")]curHp = 1 << 0, //캐릭터의 현재 체력
        [Description("타격")]attack = 1 << 1, //타격
        [Description("민첩")]agility = 1 << 2, //민첩
        [Description("방어")]defense = 1 << 3, //방어
        [Description("숙련")]skill = 1 << 4, //숙련
        [Description("회복")]recovery = 1 << 5, //회복
        [Description("내성")]tolerance = 1 << 6, //내성
        [Description("지속")]duration = 1 << 7, //지속
        [Description("수납")]acceptance = 1 << 8, //수납
        [Description("채굴")]mining = 1 << 9, //채굴
        [Description("최대 체력")]HpStat = 1 << 10, //체력 (등급)
    }
}