using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.Monster
{
    public class MonsterHpUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Slider hpSlider;
        
        public virtual void UpdateHp(float hp, int maxHp)
        {
            if (hp > 0)
            {
                hpSlider.value = (int)hp/(float)maxHp;
                hpText.text = $"{(int)hp} / {maxHp}";
            }
            else
            {
                hpSlider.value = 0;
                hpText.text = $"0 / {maxHp}";
            }
        }
    }
}