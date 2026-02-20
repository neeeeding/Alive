using System.ComponentModel;
using _02Script.Etc;
using _02Script.UI.person;
using AYellowpaper.SerializedCollections;
using UnityEditor;
using UnityEngine;

namespace _02Script.Battle
{
    [CreateAssetMenu(fileName = "BuffSO", menuName = "SO/Buff/BuffSO")]
    public class BuffSO : ScriptableObject
    {
        public bool isDeBuff = false;
        [Space(25f)]
        [Header("Buff text------------------------")]
        public BuffType buffType;
        public string buffName;
        [TextArea(3, 10)]
        public string buffExplanation;
        [Space(25f)]
        [Header("Buff do------------------------")]
        public SerializedDictionary<StatsType,float> useStatType; //사용되는 스탯, 감소 혹은 증가되는 값 (1회)
        public int repeatCount; //반복 횟수
        public float repeatDelay; //반복 딜레이
        public bool isOverlap; //중복 가능한지
        [Space(25f)]
        [Header("Buff delay------------------------")]
        public float buffDelay; //버프 지속 시간
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            string enumName = buffType.ToString();

            if (name == enumName) return;
            buffName = EnumToString.Name(buffType);

            string path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) return;

            AssetDatabase.RenameAsset(path, enumName);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public enum BuffType
    {
        //타격, 민첩, 방어, 숙련, 회복, 내성, 지속, 수납, 채굴, 체력 순
        [Description("없음")]none = 0,
        //버프
        [Description("격노")]rage = 11000,
        [Description("전투 본능")]battleInstinct,
        [Description("과충전")]overcharge,
        [Description("정밀 타격")]preciseStrike,
        [Description("광전")]berserk,
        [Description("투지")]fightingSpirit,
        [Description("돌진")]charge,
        [Description("격려")]encourage,
        [Description("궁극 각성")]ultimateAwakening,
        [Description("불꽃 강화")]flameBoost,

        [Description("가속 반응")]quickResponse = 12000,
        [Description("날렵함")]agilityBoost,
        [Description("재빠른 발")]swiftFoot,
        [Description("신속 반응")]rapidReaction,
        [Description("반사 신경")]reflexBoost,
        [Description("생존 본능")]survivalInstinct,

        [Description("철벽 태세")]ironWallStance = 13000,
        [Description("보호막")]shield,
        [Description("강철 피부")]steelSkin,
        [Description("반격 태세")]counterStance,
        [Description("안정")]stability,
        [Description("보호 의지")]protectiveWill,
        [Description("철의 의지")]ironWill,
        [Description("장비 정비")]equipmentMaintenance,

        [Description("숙련 가속")]skillAcceleration = 14000,
        [Description("각성")]awakening,
        [Description("단련 효과")]trainingEffect,
        [Description("전투 숙련")]combatMastery,

        [Description("급속 치유")]rapidHeal = 15000,
        [Description("활력")]vitality,
        [Description("활기")]energyBoost,

        [Description("인내 강화")]enduranceBoost = 16000,
        [Description("결의")]determination,
        [Description("해독")]antidote,
        [Description("정화")]purification,
        [Description("불굴")]indomitable,
        [Description("강인 체질")]toughBody,

        [Description("집중 유지")]focusMaintain = 17000,
        [Description("광채")]radiance,

        [Description("보급 확장")]supplyExpansion = 18000,
        [Description("탐욕")]greed,
        [Description("수납 확장")]inventoryExpansion,

        [Description("광부의 각성")]minerAwakening = 19000,
        [Description("수확의 손길")]harvestTouch,
        [Description("채굴 광기")]miningFrenzy,

        [Description("생명 각성")]lifeAwakening = 20000,
        [Description("체력 증강")]healthBoost,
        [Description("강인함")]fortitude,
        [Description("궁극 생명")]UltimateLife,
        
        //디버프
        
        [Description("무력화")]disarm = 51000,
        [Description("약화")]weaken,
        [Description("허약")]frailty,
        [Description("사기 저하")]demoralize,
        [Description("무기 과열")]weaponOverheat,
        [Description("무기 손상")]weaponBreak,
        [Description("독성 약화")]toxicWeakness,

        [Description("동상")]frostbite = 52000,
        [Description("혼란")]confusion,
        [Description("둔화")]slow,
        [Description("방심")]unguarded,
        [Description("결빙")]freeze,
        [Description("마비")]paralyze,
        [Description("불안정")]instability,
        [Description("무거운 갑옷")]heavyArmor,
        [Description("냉기 저주")]frostCurse,
        [Description("혼돈")]chaos,
        [Description("반응 저하")]slowReaction,

        [Description("골절")]fracture = 53000,
        [Description("공포")]fear,
        [Description("부식")]corrosion,
        [Description("방어 붕괴")]defenseBreak,
        [Description("균열")]crackArmor,
        [Description("쇠약")]debilitate,
        [Description("충격")]shockArmor,
        [Description("산성 부식")]acidCorrosion,
        [Description("장비 파손")]equipmentBreak,

        [Description("과부하")]overload = 54000,
        [Description("혼미")]daze,
        [Description("집중 붕괴")]focusBreak,
        [Description("정신 피로")]mentalFatigue,
        [Description("정신 붕괴")]mentalCollapse,
        [Description("정신 압박")]mentalPressure,

        [Description("탈진")]exhaustion = 55000,
        [Description("생기 고갈")]recoveryDrain,

        [Description("침식")]erosion = 57000,

        [Description("무거운 짐")]heavyLoad = 58000,

        [Description("피로")]fatigue = 59000,
        [Description("과로")]overwork,
        [Description("극심 피로")]severeFatigue,
        [Description("광산 피로")]miningFatigue,

        [Description("출혈")]bleeding = 60000,
        [Description("중독")]poison,
        [Description("화상")]burn,
        [Description("빈혈")]anemia,
        [Description("감염")]infection,
        [Description("파열")]rupture,
        [Description("심각한 중독")]severePoison,
        [Description("치명상")]mortalWound,
        [Description("체온")]hypothermia,
        [Description("연속 출혈")]bleedStack,
        [Description("부패")]decay,
        [Description("쇠퇴")]decline,
        [Description("생기 고갈")]lifeDrain,
        [Description("맹독")]venom,
        [Description("과출혈")]heavyBleeding,
        [Description("깊은 상처")]deepWound,
        [Description("독성 혈류")]toxicBlood,
        [Description("체력 붕괴")]hpCollapse,
        [Description("생명 부패")]lifeDecay,
        [Description("급격 출혈")]rapidBleeding,
    }
}