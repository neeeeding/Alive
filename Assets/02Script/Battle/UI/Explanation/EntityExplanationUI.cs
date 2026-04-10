using _02Script.Battle.Entity;
using _02Script.Battle.UI.Job;
using _02Script.Etc;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.Battle.UI.Explanation
{
    public class EntityExplanationUI : ExplanationUI
    {
        [Header("Setting")]
        [SerializeField] protected float _objBaseX = 3;
        [SerializeField] protected float _uiBaseX = 250;
        [Header("Need")]
        [SerializeField] protected TextMeshProUGUI hpText;
        [SerializeField] protected Slider hpSlider;
        [SerializeField] protected Camera cam;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            BattleCharacter.OnExplanation += UIShow;
            SelectCharacterCard.OnExplanation += UIShow;
            MouseExit();
        }

        protected virtual void OnDisable()
        {
            BattleCharacter.OnExplanation -= UIShow;
            SelectCharacterCard.OnExplanation -= UIShow;
        }

        protected virtual void UIShow(EntitySO so, Vector3 cardPos, bool isUI)
        {
            if (so == null)
            {
                UIHide();
                return;
            }
            EtcSet(so);
            
            SetHp(StatCalculate.Calculate(so.EntityName, StatsType.curHp), (int)StatCalculate.Calculate(so.EntityName, StatsType.HpStat));
            
            explanationText.text = $"타격 {StatCalculate.StatAlphabet(so.EntityName, StatsType.attack)}\n" +
                                   $"민첩 {StatCalculate.StatAlphabet(so.EntityName, StatsType.agility)} / 방어 {StatCalculate.StatAlphabet(so.EntityName, StatsType.defense)}\n" +
                                   $"숙련 {StatCalculate.StatAlphabet(so.EntityName, StatsType.skill)} / 회복 {StatCalculate.StatAlphabet(so.EntityName, StatsType.recovery)}\n" +
                                   $"내성 {StatCalculate.StatAlphabet(so.EntityName, StatsType.tolerance)} / 지속 {StatCalculate.StatAlphabet(so.EntityName, StatsType.duration)}\n" +
                                   $"수납 {StatCalculate.StatAlphabet(so.EntityName, StatsType.acceptance)} / 채굴 {StatCalculate.StatAlphabet(so.EntityName, StatsType.mining)}\n";

            SetPos(cardPos, isUI);
        }
        protected virtual void EtcSet(EntitySO so) //기본
        {
            if (so == null)
            {
                if(_isEnter) return;
                UIHide();
                return;
            }
            
            image.sprite = so.DialogEntityImage;
            nameText.text = EnumToString.Name(so.EntityName);
        }
        protected virtual void SetPos(Vector3 cardPos, bool isUI) //위치 지정
        {
            cardPos.x += !isUI?_objBaseX:_uiBaseX;
            if (cardPos.x > maxX)
            {
                cardPos.x -= (!isUI?_objBaseX:_uiBaseX) * 2;
            }
            UIShow(isUI ? cardPos : cam.WorldToScreenPoint(cardPos));
            _isEnter = true;
        }
        
        protected virtual void SetHp(float hp, int maxHp)
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