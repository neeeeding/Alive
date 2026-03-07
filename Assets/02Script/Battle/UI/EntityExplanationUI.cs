using _02Script.Battle.Entity;
using _02Script.Battle.UI.Etc;
using _02Script.Etc;
using _02Script.Obj.Entity;
using _02Script.UI.person;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI
{
    public class EntityExplanationUI : ExplanationUI
    {
        [Header(("Setting"))]
        [SerializeField] private float _baseX = 20;
        [Header("Need")]
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Slider hpSlider;
        [SerializeField] protected Camera cam;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            BattleCharacter.OnExplanation += UIShow;
            MouseExit();
        }

        private void OnDisable()
        {
            BattleCharacter.OnExplanation -= UIShow;
        }

        private void UIShow(EntitySO so, Vector3 cardPos, bool isUI)
        {
            if (so == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }
            
            image.sprite = so.DialogEntityImage;
            nameText.text = EnumToString.Name(so.EntityName);
            
            SetHp(StatCalculate.Calculate(so.EntityName, StatsType.curHp), (int)StatCalculate.Calculate(so.EntityName, StatsType.HpStat));
            
            explanationText.text = $"타격 {StatCalculate.StatAlphabet(so.EntityName, StatsType.attack)}\n" +
                                   $"민첩 {StatCalculate.StatAlphabet(so.EntityName, StatsType.agility)} / 방어 {StatCalculate.StatAlphabet(so.EntityName, StatsType.defense)}\n" +
                                   $"숙련 {StatCalculate.StatAlphabet(so.EntityName, StatsType.skill)} / 회복 {StatCalculate.StatAlphabet(so.EntityName, StatsType.recovery)}\n" +
                                   $"내성 {StatCalculate.StatAlphabet(so.EntityName, StatsType.tolerance)} / 지속 {StatCalculate.StatAlphabet(so.EntityName, StatsType.duration)}\n" +
                                   $"수납 {StatCalculate.StatAlphabet(so.EntityName, StatsType.acceptance)} / 채굴 {StatCalculate.StatAlphabet(so.EntityName, StatsType.mining)}\n";

            cardPos.x += _baseX;
            UIShow(isUI ? cardPos : cam.WorldToScreenPoint(cardPos));
            _isEnter = true;
        }
        
        private void SetHp(float hp, int maxHp)
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