using System.ComponentModel;
using UnityEngine;

namespace _02Script.Obj.Character
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "SO/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        public CharacterName characterName;
        [Space(10f)] public PlayerJob characterJob;
        [Space(15f)] public Sprite characterImage;
        [Space(20f)] [Header("Dialog")] public TextAsset[] characterDialog;
    }

    public enum CharacterName
    {
        [Description("주인공")] me,
        [Description("레스티")] resty,
        [Description("크리스")] chris,
        [Description("피오")] pio,
        [Description("테오")] theo,
        [Description("노아")] noah,
        [Description("니아")] nia,
        [Description("빌런")] villain,
        [Description("해리")] harry,
        [Description("다니엘")] daniel
    }

    public enum PlayerJob
    {
        [Description("없음")] none = 0,
        [Description("흑")] brack,
        [Description("치료")] heal,
        [Description("불")] fire,
        [Description("물")] water,
        [Description("복제")] copy,
        [Description("포션")] potion
    }
}