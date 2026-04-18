using System;
using System.ComponentModel;
using _02Script.Etc;
using _02Script.Obj.Entity;
using _02Script.UI.Person;
using UnityEditor;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemDataSO")]
    public class ItemDataSO : ScriptableObject
    {
        public static Action<EntityName,StatsType, int> OnStats;

        [Space(25f)]
        [Header("ItemType------------------------")]
        public ItemCategory category; //카테고리
        public ItemType itemType; //아이템 종류

        [Space(25f)]
        [Header("Item text------------------------")]
        public string itemName;
        [TextArea(3, 10)]
        public string itemExplanation;
        
        [Space(25f)]
        [Header("Item Need------------------------")]
        public Sprite itemImage;
        public int maxCount;
        public float collectTime = 2; //얻는데 걸리는 시간
        
        [Space(25f)]
        [Header("Food Need------------------------")]
        public StatsType stats = StatsType.curHp;
        public int addStats;
        
        public bool DoSomething(EntityName entity) //보통은 그냥 사용 못하게
        {
            switch (category)
            {
                case ItemCategory.food :
                    OnStats?.Invoke(entity,stats, addStats);
                    return true;
            }
            return false;
        }
        
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            // string enumName = itemType.ToString();
            //
            // if (name == enumName) return;
            // //if (itemName == EnumToString.Name(itemType)) return;
            //
            // itemName = EnumToString.Name(itemType);
            //
            // string path = AssetDatabase.GetAssetPath(this);
            // if (string.IsNullOrEmpty(path)) return;
            //
            // AssetDatabase.RenameAsset(path, enumName);
            // AssetDatabase.SaveAssets();
        }
#endif
    }

    public enum ItemCategory //카테고리
    {
        [Description("없음")]none = 0,
        
        [Description("씨앗")]seed = 1,
        [Description("식품")]viand = 2,
        [Description("음식")]food = 3,
        
        [Description("무기")]weapon = 4,
        [Description("갑옷")]armor = 5,
        
        [Description("부품")]stuff = 6,
        [Description("기계")]machine = 7,
        [Description("기타")]special = 8,
        
        [Description("책")]book = 99,
    }

    //엉엉엉엉 ㅠㅠㅠㅠㅠ CSV로 할 걸... 이미 많이 늦었다...
    public enum ItemType //종류
    {
        [Description("없음")]none = 0, //없다.

        [Description("벼 씨앗")]riceSeeds = 1001,
        [Description("당근 씨앗")]carrotSeeds = 1002,
        [Description("파 씨앗")]greenOnionSeeds = 1003,
        [Description("양파 씨앗")]onionSeeds = 1004,
        [Description("오이 씨앗")]cucumberSeeds = 1005,
        [Description("콩나물 씨앗")]beanSproutSeeds = 1006,
        [Description("시금치 씨앗")]spinachSeeds = 1007,
        [Description("고사리 종근")]brackenSeeds = 1008,
        [Description("우엉 씨앗")]burdockSeeds = 1009,
        [Description("무 씨앗")]radishSeeds = 1010,
        [Description("애호박 씨앗")]squashSeeds = 1011,
        [Description("깨 씨앗")]sesameSeeds = 1012,
        [Description("버섯 씨앗")]mushroomSeeds = 1013,
        
        [Description("토마토 씨앗")]tomatoSeeds = 1015,
        [Description("상추 씨앗")]lettuceSeeds = 1016,
        [Description("감자 씨앗")]potatoSeeds = 1017,
        [Description("콩 씨앗")]beanSeeds = 1018,
        [Description("고추 씨앗")]pepperSeeds = 1019,
        [Description("양배추 씨앗")]cabbageSeeds = 1020,
        [Description("팥 씨앗")]redBeanSeeds = 1021,
        [Description("딸기 씨앗")]strawberrySeeds = 1022,
        [Description("올리브 씨앗")]oliveSeeds = 1023,
        [Description("블루베리 씨앗")]blueberrySeeds = 1024,
        [Description("밀 씨앗")]wheatSeeds = 1025,
        [Description("옥수수 씨앗")]cornerSeeds = 1026,
        [Description("사과 씨앗")]appleSeeds = 1027,
        [Description("포도 씨앗")]grapeSeeds = 1028,
        [Description("커피 씨앗")]coffeeSeeds = 1029,
        [Description("바나나 흡아")]bananaSeeds = 1030,
        [Description("망고 씨앗")]mangoSeeds = 1031,
        [Description("코코넛 씨앗")]coconutSeeds = 1032,
        [Description("사탕수수 씨앗")]sugarCaneSeeds = 1033,
        [Description("카카오 씨앗")]cacaoSeeds = 1034,
        [Description("아몬드 씨앗")]almondSeeds = 1035,
        [Description("브로콜리 씨앗")]broccoliSeeds = 1036,
        
        //재료 (100 씩 분류)
        [Description("벼")]rice = 2001,
        [Description("당근")]carrot = 2002,
        [Description("파")]greenOnion = 2003,
        [Description("양파")]onion = 2004,
        [Description("오이")]cucumber = 2005,
        [Description("콩나물")]beanSprout = 2006,
        [Description("시금치")]spinach = 2007,
        [Description("고사리")]bracken = 2008,
        [Description("우엉")]burdock = 2009,
        [Description("무")]radish = 2010,
        [Description("애호박")]squash = 2011,
        [Description("깨")]sesame = 2012,
        [Description("버섯")]mushroom = 2013,
        
        [Description("토마토")]tomato = 2015,
        [Description("상추")]lettuce = 2016,
        [Description("감자")]potato = 2017,
        [Description("콩")]bean = 2018,
        [Description("고추")]pepper = 2019,
        [Description("양배추")]cabbage = 2020,
        [Description("팥")]redBean = 2021,
        [Description("딸기")]strawberry = 2022,
        [Description("올리브")]olive = 2023,
        [Description("블루베리")]blueberry = 2024,
        [Description("밀")]wheat = 2025,
        [Description("옥수수")]corner = 2026,
        [Description("사과")]apple = 2027,
        [Description("포도")]grape = 2028,
        [Description("커피")]coffee = 2029,
        [Description("바나나")]banana = 2030,
        [Description("망고")]mango = 2031,
        [Description("코코넛")]coconut = 2032,
        [Description("사탕수수")]sugarCane = 2033,
        [Description("카카오")]cacao = 2034,
        [Description("아몬드")]almond = 2035,
        [Description("브로콜리")]broccoli = 2036,
        
        [Description("전분")]starch = 2201,
        [Description("밀가루")]flour = 2202,
        [Description("면")]noodle = 2203,
        
        [Description("춘장")]chunjang = 2211,
        [Description("된장")]soybeanPaste = 2212,
        [Description("두부")]tofu = 2213,
        
        [Description("치즈")]cheese = 2221,
        [Description("버터")]butter = 2222,
        [Description("휘핑 크림")]whippedCream = 2223,
        
        [Description("라면")]ramenNot = 2231,
        [Description("카레 가루")]curryPowder = 2232,
        
        [Description("설탕")]sugar = 2241,
        [Description("꿀")]honey = 2242,
        
        [Description("닭 고기")]chicken = 2501,
        [Description("소고기")]caw = 2502,
        [Description("돼지 고기")]pig = 2503,
        [Description("고등어")]mackerel = 2504,
        [Description("낙지")]octopusSmall = 2505,
        [Description("계란")]egg = 2506,
        [Description("우유")]milk = 2507,
        
        //요리 (빈, 프라이팬, 냄비, 오븐,밥솥 전자레인지)
        [Description("초 밥")]fishRice = 3101,
        [Description("비빔밥")]bibimRice = 3102,
        
        [Description("떡")]riceCake = 3111,
        [Description("피클")]pickle = 3151,
        
        [Description("딸기 스무디")]strawberrySmoothie = 3121,
        [Description("아이스크림")]iceCream = 3141,
        [Description("햄버거")]hamburger = 3161,
        //
        [Description("짜장면")]jjajangmyeon = 3201,
        [Description("낙지 볶음")]StirFriedOctopus = 3202,
        
        [Description("계란 말이")]eggRoll = 3221,
        
        [Description("돈까스")]porkCutlet = 3241,
        //
        [Description("고등어 조림")]stewedMackerel = 3301,
        [Description("마파 두부")]mapoTofu = 3302,
        
        [Description("찐만두")]steamedDumplings = 3321,
        [Description("갈비찜")]steamedRib = 3322,
        [Description("계란찜")]steamedEgg = 3323,
        
        [Description("된장국")]misoSoup = 3341,
        [Description("닭도리탕")]chickenDoritang = 3342,
        
        [Description("팥죽")]redBeanPorridge = 3361,
        
        [Description("카레")]curry = 3371,
        [Description("라면")]ramen = 3372,
        //
        [Description("빵")]bread = 3401,
        [Description("피자")]pizza = 3402,
        [Description("마들렌")]madeleine = 3403,
        [Description("케이크")]cake = 3451 ,
        //
        [Description("흰 밥")]warmRice = 3601,
        [Description("콩 밥")]beanRice = 3602,
        
        //무기 (100씩 분류) ( 특수(비림이니, 블리베루...),근접, 원거리, 투척, 특수 무기, 중화기)
        [Description("바주카")] bazooka = 4001, //M9A1
        [Description("비림이니")] biriminini = 4101,
        [Description("전투용 블리베루")] battleBliveru = 4201,
        
        [Description("무딘 칼")]justKnife = 4401,
        [Description("날카로운 검")]sharpSword,
        [Description("무진장 무거운 대검")]heavyGreatsword,
        [Description("은밀한 단검")]stealthDagger,
        [Description("귀족의 레이피어")]nobleRapier,
        [Description("녹슨 도끼")]rustyAxe,
        [Description("둔한 망치")]bluntHammer,
        [Description("심판의 철퇴")]judgementMace,
        [Description("싸구려 야구 방망이")]cheapBat,
        [Description("갈라진 나무 몽둥이")]splitClub,
        [Description("봉 창")]spear,
        [Description("오래된 할버드")]oldHalberd,
        [Description("고통의 채찍")]painWhip,
        [Description("격투가의 너클")]fighterKnuckle,
        [Description("사슬 철퇴")]chainMace,
        [Description("집행자의 낫")]executionerScythe,

        [Description("가벼운 활")]lightBow = 4501,
        [Description("장전된 석궁")]loadedCrossbow,
        [Description("날으는 투창")]flyingJavelin,
        [Description("회전하는 수리검")]spinningShuriken,
        [Description("목동의 투석구")]shepherdSling,

        [Description("낡은 단총")]oldPistol = 4601,
        [Description("보안관의 리볼버")]sheriffRevolver,
        [Description("전장의 기관총")]battleMachineGun,
        [Description("군용 소총")]militaryRifle,
        [Description("근접용 샷건")]combatShotgun,

        [Description("표준 수류탄")]standardGrenade = 4701,
        [Description("불붙은 화염병")]molotovCocktail,
        [Description("검은 폭탄")]blackBomb,
        [Description("날렵한 쿠나이")]swiftKunai,

        [Description("자유로운 화염 방사기")]flamethrower = 4801,
        [Description("고압 전기 건")]electricGun,
        
        // 방어구 (100씩 분류) 머리, 몸통, 팔, 다리, 신발, 기타
        [Description("견고한 철제 투구")]ironHelmet = 5001,
        [Description("가벼운 가죽 모자")]leatherCap,
        [Description("기사의 투구")]knightHelmet,
        [Description("충격 흡수 헬멧")]shockAbsorbHelmet,

        [Description("강화 가죽 갑옷")]reinforcedLeatherArmor = 5101,
        [Description("전사의 판금 갑옷")]warriorPlateArmor,
        [Description("수호자의 중갑")]guardianHeavyArmor,
        [Description("경량 전투복")]lightCombatSuit,
        [Description("과부하 방어복")]overloadArmor,

        [Description("전투용 장갑")]combatGloves = 5201,
        [Description("강철 건틀릿")]steelGauntlet,
        [Description("격투가의 장갑")]fighterGloves,
        [Description("방어용 건틀릿")]defenseGauntlet,
        [Description("충격 반응 장갑")]shockReactiveGloves,

        [Description("전투용 하의")]combatPants = 5301,
        [Description("기사의 하갑")]knightLegArmor,
        [Description("경량 하의")]lightPants,
        [Description("중장 하의")]heavyLegArmor,
        [Description("불안정 하의")]unstablePants,

        [Description("가벼운 부츠")]lightBoots = 5401,
        [Description("강철 부츠")]steelBoots,
        [Description("기사의 부츠")]knightBoots,
        [Description("충격 흡수 부츠")]shockAbsorbBoots,
        [Description("마모된 부츠")]wornBoots,
        
        [Description("방패")]justShield = 5501,
        
        //자연 (20씩 분류)
        [Description("자갈")]gravel = 6001,
        [Description("점토")]clay = 6002,
        [Description("모래")]sand = 6003,
        [Description("석영")]quartz = 6004,
        [Description("석유")]petroleum = 6005,
        [Description("흑연")]blackSmoke = 6006,
        [Description("산")]poison = 6007,
        [Description("기름")]oil = 6008,
        
        [Description("철 광석")]ironStone = 6021,
        [Description("구리 광석")]copperStone = 6022,
        [Description("금 광석")]goldStone = 6023,
        [Description("알루미늄 광석")]aluminumStone = 6024,
        [Description("크롬 광석")]chromeStone = 6025,
        
        [Description("원목")]wood = 6041,
        [Description("목화")]cotton = 6042,
        [Description("고무")]rubber = 6043,
        
        [Description("루비")]ruby = 6061,
        [Description("사파이어")]sapphire = 6062,
        [Description("에메랄드")]emerald = 6063,
        [Description("다이아몬드")]diamond = 6064,
        [Description("자수정")]amethyst = 6065,
        [Description("토파즈")]topaz = 6066,
        [Description("오팔")]opal = 6067,
        [Description("흑요석")]obsidian = 6068,
        [Description("호박")]pumpkinJewel = 6069,
        [Description("수정")]correction = 6070,
        
        //가공 (20씩 분류)
        [Description("철")]iron = 6101,
        [Description("구리")]copper = 6102,
        [Description("금")]gold = 6103,
        [Description("알루미늄")]aluminum = 6104,
        [Description("스테인리스강")]stainless = 6105,
        
        [Description("전선")]electricWire = 6121,
        [Description("구리 코일")]copperCoil = 6122,
        [Description("회로기판")]circuitBoard = 6123,
        [Description("반도체")]semiconductor = 6124,
        [Description("콘덴서")]capacitor = 6125,
        [Description("트랜지스터")]transistor = 6126,
        [Description("배터리")]battery = 6127,
        [Description("절연체")]insulator = 6128,
        [Description("자석")]magnet = 6129,
        
        [Description("벽돌")]brick = 6141,
        [Description("콘크리트")]concrete = 6142,
        [Description("시멘트")]cement = 6143,
        [Description("석재")]ston = 6144,
        [Description("철근")]rebar = 6145,
        [Description("단열재")]insulation = 6146,
        [Description("방수재")]waterproofing = 6147,
        
        [Description("천")]cloth = 6161,
        [Description("가죽")]leather = 6162,
        [Description("실")]thread = 6163,
        [Description("섬유")]naturalFiber = 6164,
        
        [Description("유리")]glass = 6181,
        [Description("판자")]board = 6182,
        [Description("플라스틱")]profit = 6183,
        [Description("나사")]screw = 6184,
        
        //기계
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