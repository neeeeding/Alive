using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace _02Script.Obj.Entity
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/Entity/Entity")]
    public class EntitySO : ScriptableObject
    {
        public EntityName EntityName;
        [Space(15f)]
        public Sprite DialogEntityImage;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            string enumName = EntityName.ToString();

            if (name == enumName) return;

            string path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) return;

            AssetDatabase.RenameAsset(path, enumName);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public enum EntityName
    {
        [Description("오류 없음")] None = -1,
        
        [Description("라이")] lie = 10000,
        [Description("최태한")] taehan = 11000,
        [Description("서도한")] dohan =  12000,
        [Description("도하준")] hajun = 13000,
        
        [Description("이시스")] isis = 20000,
        [Description("레이")] ray = 21000,
        [Description("마젠타")] magenta = 22000,
        [Description("라엘리아")] raelia = 23000,
        
        //몬스터를 종족에 따라 분류?? (주석)
        [Description("토끼")] rabbit = 30000,
        [Description("게")] crab = 30100,
        
        //40000 대는 오류
    }
}