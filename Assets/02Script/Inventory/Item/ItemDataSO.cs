using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemDataSO")]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite itemImage;
        public int maxCount;

        public ItemCategory category; //카테고리
        public ItemType itemType; //아이템 종류

        public string itemName;
        [TextArea(3, 10)]
        public string itemExplanation;
        
        public bool DoSomething() //보통은 그냥 사용 못하게
        {
            switch (category)
            {
                case ItemCategory.food :
                    // 먹을 상대 정하기
                    return true;
            }
            return false;
        }
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            string enumName = itemType.ToString();

            if (name == enumName) return;

            string path = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) return;

            AssetDatabase.RenameAsset(path, enumName);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    public enum ItemCategory //카테고리
    {
        [Description("없음")]none = 0,
        
        [Description("씨앗")]seed = 1,
        [Description("곡물")]fruit = 2,
        [Description("음식")]food = 3,
        
        [Description("무기")]weapon = 4,
        [Description("갑옷")]armor = 5,
        
        [Description("부품")]stuff = 6,
        [Description("기계")]machine = 7,
        [Description("기타")]special = 8,
    }

    public enum ItemType //종류
    {
        [Description("없음")]none = 0, //없다.

        [Description("벼 씨앗")]riceSeeds = 1001,
        [Description("벼")]rice = 2001,
        [Description("쌀 밥")]warmRice = 3001,
        
        [Description("칼")]justKnife = 4001,
        [Description("방패")]justShield = 5001,
        
        [Description("나사")]screw = 6001,
        [Description("블리베루")] bliveru = 7001,
        
        [Description("빈")] notting = 7100,
        [Description("프라이팬")] fryingPan = 7101,
        [Description("냄비")] pot = 7102,
        [Description("오븐")] oven = 7103,
        [Description("밥솥")] riceCooker = 7104,
        [Description("전자레인지")] microwaveOven = 7105,
        
        [Description("톱")] saw = 7110,
        [Description("드라이버")] screwdriver = 7111,
        [Description("용접기")] welder = 7112,
        [Description("용해로")] furnace = 7113,
        [Description("유리 블로우 파이프")] blowPipe = 7114,
        [Description("보석 절단기")] jewelCutter = 7115,
        [Description("패싯 머신")] facetMachine = 7116,
        [Description("발전기")] generator = 7117,
        [Description("납땜인두")] solderingIron = 7118,
        [Description("와이어 커터")] wireCutter = 7119,
        [Description("발물레")]  spinningWheel = 7120,
        
        [Description("별 모양 보석")]starJewelry = 8001,
    }
}