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
            hpSlider.value = (int)hp/(float)maxHp;
            hpText.text = $"{(int)hp}/{maxHp}";
        }
    }
}