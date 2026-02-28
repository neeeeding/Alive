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
        
        // 해 / 육 / 공 / 독 / 곤충 / 보스
        [Description("게")]crab = 30000,
        [Description("전기 뱀장어")]electricEel,
        [Description("기생 물고기")]parasiticFish,

        [Description("근육 토끼")]muscleRabbit = 31000,
        [Description("돌멧돼지")]stoneBoar,
        [Description("주머니 도마뱀")]pouchLizard,
        [Description("부패 사슴")]rottenDeer,

        [Description("뼈 까마귀")]boneCrow = 32000,
        [Description("천둥 매")]thunderHawk,
        [Description("알비노 비둘기")]albinoPigeon,

        [Description("독버섯")]poisonMushroom = 33000,
        [Description("진흙 슬라임")]mudSlime,
        [Description("피식물 덩굴")]bloodVine,
        [Description("곰팡이 해파리")]fungusJellyfish,

        [Description("모래 벌레")]sandWorm = 34000,
        [Description("독가스 두꺼비")]toxicGasToad,
        [Description("유리 나비")]glassButterfly,

        [Description("불꽃 도마뱀 늑대")]flameLizardWolf = 35000,
        [Description("거미 토끼")]spiderRabbit,
        [Description("독침 벌 곰")]stingerBeeBear,
        [Description("상어 고릴라")]sharkGorilla,
        [Description("심연 촉수")]abyssTentacle,
        
        //40000 대는 오류
    }
}