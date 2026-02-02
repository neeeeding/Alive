using System.ComponentModel;
using _02Script.Etc;
using _02Script.UI.person;
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
        public StatsType useStatType; //사용되는 스탯
        public float addStat; //감소 혹은 증가되는 값 (1회)
        public int repeatCount; //반복 횟수
        public float repeatDelay; //반복 딜레이
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
        [Description("없음")]none = 0,
    }
}