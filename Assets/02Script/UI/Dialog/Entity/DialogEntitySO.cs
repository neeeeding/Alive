using System.ComponentModel;
using UnityEngine;

namespace _02Script.UI.Dialog.Entity
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/CharacterSO")]
    public class DialogEntitySO : ScriptableObject
    {
        public EntityName EntityName;
        [Space(15f)]
        public Sprite DialogEntityImage;
        [Space(20f)]
        [Header("Dialog")]
        public TextAsset[] DialogTextFile;
        //public SerializedDictionary<string, ItemDataSO> Items; //가지고 있을 아이템들
    }

    public enum EntityName
    {
        [Description("오류 없음")] None = -1,
        
        [Description("라이")] lie = 1000,
        [Description("최태한")] taehan = 1100,
        [Description("서도한")] dohan =  1200,
        [Description("도하준")] hajun = 1300,
        
        [Description("이시스")] isis = 2000,
        [Description("레이")] ray = 2100,
        [Description("마젠타")] magenta = 2200,
        [Description("라엘리아")] raelia = 2300,
        
        //4000 대는 오류
    }
}