using System;
using System.ComponentModel;
using _02Script.UI.person;
using UnityEditor;
using UnityEngine;

namespace _02Script.Inventory.Item
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemDataSO")]
    public class ItemDataSO : ScriptableObject
    {
        public static Action<StatsType, int> OnStats;

        [Space(25f)]
        [Header("ItemType------------------------")]
        public ItemCategory category; //Ä«Å×°í¸®
        public ItemType itemType; //¾ÆÀÌÅÛ Á¾·ù

        [Space(25f)]
        [Header("Item text------------------------")]
        public string itemName;
        [TextArea(3, 10)]
        public string itemExplanation;
        
        [Space(25f)]
        [Header("Item Need------------------------")]
        public Sprite itemImage;
        public int maxCount;
        public float collectTime = 2; //¾ò´Âµ¥ °É¸®´Â ½Ã°£
        
        [Space(25f)]
        [Header("Food Need------------------------")]
        public StatsType stats = StatsType.curHp;
        public int addStats;
        
        public bool DoSomething() //º¸ÅëÀº ±×³É »ç¿ë ¸øÇÏ°Ô
        {
            switch (category)
            {
                case ItemCategory.food :
                    OnStats?.Invoke(stats, addStats);
                    return true;
            }
            return false;
        }
        
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
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

    public enum ItemCategory //Ä«Å×°í¸®
    {
        [Description("¾øÀ½")]none = 0,
        
        [Description("¾¾¾Ñ")]seed = 1,
        [Description("½ÄÇ°")]viand = 2,
        [Description("À½½Ä")]food = 3,
        
        [Description("¹«±â")]weapon = 4,
        [Description("°©¿Ê")]armor = 5,
        
        [Description("ºÎÇ°")]stuff = 6,
        [Description("±â°è")]machine = 7,
        [Description("±âÅ¸")]special = 8,
        
        [Description("Ã¥")]book = 99,
    }

    //¾û¾û¾û¾û ¤Ð¤Ð¤Ð¤Ð¤Ð CSV·Î ÇÒ °É... ÀÌ¹Ì ¸¹ÀÌ ´Ê¾ú´Ù...
    public enum ItemType //Á¾·ù
    {
        [Description("¾øÀ½")]none = 0, //¾ø´Ù.

        [Description("º­ ¾¾¾Ñ")]riceSeeds = 1001,
        [Description("´ç±Ù ¾¾¾Ñ")]carrotSeeds = 1002,
        [Description("ÆÄ ¾¾¾Ñ")]greenOnionSeeds = 1003,
        [Description("¾çÆÄ ¾¾¾Ñ")]onionSeeds = 1004,
        [Description("¿ÀÀÌ ¾¾¾Ñ")]cucumberSeeds = 1005,
        [Description("Äá³ª¹° ¾¾¾Ñ")]beanSproutSeeds = 1006,
        [Description("½Ã±ÝÄ¡ ¾¾¾Ñ")]spinachSeeds = 1007,
        [Description("°í»ç¸® Á¾±Ù")]brackenSeeds = 1008,
        [Description("¿ì¾û ¾¾¾Ñ")]burdockSeeds = 1009,
        [Description("¹« ¾¾¾Ñ")]radishSeeds = 1010,
        [Description("¾ÖÈ£¹Ú ¾¾¾Ñ")]squashSeeds = 1011,
        [Description("±ú ¾¾¾Ñ")]sesameSeeds = 1012,
        [Description("¹ö¼¸ ¾¾¾Ñ")]mushroomSeeds = 1013,
        
        [Description("Åä¸¶Åä ¾¾¾Ñ")]tomatoSeeds = 1015,
        [Description("»óÃß ¾¾¾Ñ")]lettuceSeeds = 1016,
        [Description("°¨ÀÚ ¾¾¾Ñ")]potatoSeeds = 1017,
        [Description("Äá ¾¾¾Ñ")]beanSeeds = 1018,
        [Description("°íÃß ¾¾¾Ñ")]pepperSeeds = 1019,
        [Description("¾ç¹èÃß ¾¾¾Ñ")]cabbageSeeds = 1020,
        [Description("ÆÏ ¾¾¾Ñ")]redBeanSeeds = 1021,
        [Description("µþ±â ¾¾¾Ñ")]strawberrySeeds = 1022,
        [Description("¿Ã¸®ºê ¾¾¾Ñ")]oliveSeeds = 1023,
        [Description("ºí·çº£¸® ¾¾¾Ñ")]blueberrySeeds = 1024,
        [Description("¹Ð ¾¾¾Ñ")]wheatSeeds = 1025,
        [Description("¿Á¼ö¼ö ¾¾¾Ñ")]cornerSeeds = 1026,
        [Description("»ç°ú ¾¾¾Ñ")]appleSeeds = 1027,
        [Description("Æ÷µµ ¾¾¾Ñ")]grapeSeeds = 1028,
        [Description("Ä¿ÇÇ ¾¾¾Ñ")]coffeeSeeds = 1029,
        [Description("¹Ù³ª³ª Èí¾Æ")]bananaSeeds = 1030,
        [Description("¸Á°í ¾¾¾Ñ")]mangoSeeds = 1031,
        [Description("ÄÚÄÚ³Ó ¾¾¾Ñ")]coconutSeeds = 1032,
        [Description("»çÅÁ¼ö¼ö ¾¾¾Ñ")]sugarCaneSeeds = 1033,
        [Description("Ä«Ä«¿À ¾¾¾Ñ")]cacaoSeeds = 1034,
        [Description("¾Æ¸óµå ¾¾¾Ñ")]almondSeeds = 1035,
        [Description("ºê·ÎÄÝ¸® ¾¾¾Ñ")]broccoliSeeds = 1036,
        
        //Àç·á (100 ¾¿ ºÐ·ù)
        [Description("º­")]rice = 2001,
        [Description("´ç±Ù")]carrot = 2002,
        [Description("ÆÄ")]greenOnion = 2003,
        [Description("¾çÆÄ")]onion = 2004,
        [Description("¿ÀÀÌ")]cucumber = 2005,
        [Description("Äá³ª¹°")]beanSprout = 2006,
        [Description("½Ã±ÝÄ¡")]spinach = 2007,
        [Description("°í»ç¸®")]bracken = 2008,
        [Description("¿ì¾û")]burdock = 2009,
        [Description("¹«")]radish = 2010,
        [Description("¾ÖÈ£¹Ú")]squash = 2011,
        [Description("±ú")]sesame = 2012,
        [Description("¹ö¼¸")]mushroom = 2013,
        
        [Description("Åä¸¶Åä")]tomato = 2015,
        [Description("»óÃß")]lettuce = 2016,
        [Description("°¨ÀÚ")]potato = 2017,
        [Description("Äá")]bean = 2018,
        [Description("°íÃß")]pepper = 2019,
        [Description("¾ç¹èÃß")]cabbage = 2020,
        [Description("ÆÏ")]redBean = 2021,
        [Description("µþ±â")]strawberry = 2022,
        [Description("¿Ã¸®ºê")]olive = 2023,
        [Description("ºí·çº£¸®")]blueberry = 2024,
        [Description("¹Ð")]wheat = 2025,
        [Description("¿Á¼ö¼ö")]corner = 2026,
        [Description("»ç°ú")]apple = 2027,
        [Description("Æ÷µµ")]grape = 2028,
        [Description("Ä¿ÇÇ")]coffee = 2029,
        [Description("¹Ù³ª³ª")]banana = 2030,
        [Description("¸Á°í")]mango = 2031,
        [Description("ÄÚÄÚ³Ó")]coconut = 2032,
        [Description("»çÅÁ¼ö¼ö")]sugarCane = 2033,
        [Description("Ä«Ä«¿À")]cacao = 2034,
        [Description("¾Æ¸óµå")]almond = 2035,
        [Description("ºê·ÎÄÝ¸®")]broccoli = 2036,
        
        [Description("ÀüºÐ")]starch = 2201,
        [Description("¹Ð°¡·ç")]flour = 2202,
        [Description("¸é")]noodle = 2203,
        
        [Description("ÃáÀå")]chunjang = 2211,
        [Description("µÈÀå")]soybeanPaste = 2212,
        [Description("µÎºÎ")]tofu = 2213,
        
        [Description("Ä¡Áî")]cheese = 2221,
        [Description("¹öÅÍ")]butter = 2222,
        [Description("ÈÖÇÎ Å©¸²")]whippedCream = 2223,
        
        [Description("¶ó¸é")]ramenNot = 2231,
        [Description("Ä«·¹ °¡·ç")]curryPowder = 2232,
        
        [Description("¼³ÅÁ")]sugar = 2241,
        [Description("²Ü")]honey = 2242,
        
        [Description("´ß °í±â")]chicken = 2501,
        [Description("¼Ò°í±â")]caw = 2502,
        [Description("µÅÁö °í±â")]pig = 2503,
        [Description("°íµî¾î")]mackerel = 2504,
        [Description("³«Áö")]octopusSmall = 2505,
        [Description("°è¶õ")]egg = 2506,
        [Description("¿ìÀ¯")]milk = 2507,
        
        //¿ä¸® (ºó, ÇÁ¶óÀÌÆÒ, ³¿ºñ, ¿Àºì,¹ä¼Ü ÀüÀÚ·¹ÀÎÁö)
        [Description("ÃÊ ¹ä")]fishRice = 3101,
        [Description("ºñºö¹ä")]bibimRice = 3102,
        
        [Description("¶±")]riceCake = 3111,
        [Description("ÇÇÅ¬")]pickle = 3151,
        
        [Description("µþ±â ½º¹«µð")]strawberrySmoothie = 3121,
        [Description("¾ÆÀÌ½ºÅ©¸²")]iceCream = 3141,
        [Description("ÇÜ¹ö°Å")]hamburger = 3161,
        //
        [Description("Â¥Àå¸é")]jjajangmyeon = 3201,
        [Description("³«Áö ººÀ½")]StirFriedOctopus = 3202,
        
        [Description("°è¶õ ¸»ÀÌ")]eggRoll = 3221,
        
        [Description("µ·±î½º")]porkCutlet = 3241,
        //
        [Description("°íµî¾î Á¶¸²")]stewedMackerel = 3301,
        [Description("¸¶ÆÄ µÎºÎ")]mapoTofu = 3302,
        
        [Description("Âð¸¸µÎ")]steamedDumplings = 3321,
        [Description("°¥ºñÂò")]steamedRib = 3322,
        [Description("°è¶õÂò")]steamedEgg = 3323,
        
        [Description("µÈÀå±¹")]misoSoup = 3341,
        [Description("´ßµµ¸®ÅÁ")]chickenDoritang = 3342,
        
        [Description("ÆÏÁ×")]redBeanPorridge = 3361,
        
        [Description("Ä«·¹")]curry = 3371,
        [Description("¶ó¸é")]ramen = 3372,
        //
        [Description("»§")]bread = 3401,
        [Description("ÇÇÀÚ")]pizza = 3402,
        [Description("¸¶µé·»")]madeleine = 3403,
        [Description("ÄÉÀÌÅ©")]cake = 3451 ,
        //
        [Description("Èò ¹ä")]warmRice = 3601,
        [Description("Äá ¹ä")]beanRice = 3602,
        
        //¹«±â (100¾¿ ºÐ·ù) (Ä®·ù,¹ß»çÃ¼(ÃÑ·ù), 
        [Description("¹«µò Ä®")]justKnife = 4001,
        [Description("ºñ¸²ÀÌ´Ï")] biriminini = 4002,
        
        [Description("¹ÙÁÖÄ«")] bazooka = 4101, //M9A1
        
        [Description("¹æÆÐ")]justShield = 5001,
        
        //ÀÚ¿¬ (20¾¿ ºÐ·ù)
        [Description("ÀÚ°¥")]gravel = 6001,
        [Description("Á¡Åä")]clay = 6002,
        [Description("¸ð·¡")]sand = 6003,
        [Description("¼®¿µ")]quartz = 6004,
        [Description("¼®À¯")]petroleum = 6005,
        [Description("Èæ¿¬")]blackSmoke = 6006,
        [Description("»ê")]poison = 6007,
        
        [Description("Ã¶ ±¤¼®")]ironStone = 6021,
        [Description("±¸¸® ±¤¼®")]copperStone = 6022,
        [Description("±Ý ±¤¼®")]goldStone = 6023,
        [Description("¾Ë·ç¹Ì´½ ±¤¼®")]aluminumStone = 6024,
        [Description("Å©·Ò ±¤¼®")]chromeStone = 6025,
        
        [Description("¿ø¸ñ")]wood = 6041,
        [Description("¸ñÈ­")]cotton = 6042,
        [Description("°í¹«")]rubber = 6043,
        
        [Description("·çºñ")]ruby = 6061,
        [Description("»çÆÄÀÌ¾î")]sapphire = 6062,
        [Description("¿¡¸Þ¶öµå")]emerald = 6063,
        [Description("´ÙÀÌ¾Æ¸óµå")]diamond = 6064,
        [Description("ÀÚ¼öÁ¤")]amethyst = 6065,
        [Description("ÅäÆÄÁî")]topaz = 6066,
        [Description("¿ÀÆÈ")]opal = 6067,
        [Description("Èæ¿ä¼®")]obsidian = 6068,
        [Description("È£¹Ú")]pumpkinJewel = 6069,
        [Description("¼öÁ¤")]correction = 6070,
        
        //°¡°ø (20¾¿ ºÐ·ù)
        [Description("Ã¶")]iron = 6101,
        [Description("±¸¸®")]copper = 6102,
        [Description("±Ý")]gold = 6103,
        [Description("¾Ë·ç¹Ì´½")]aluminum = 6104,
        [Description("½ºÅ×ÀÎ¸®½º°­")]stainless = 6105,
        
        [Description("Àü¼±")]electricWire = 6121,
        [Description("±¸¸® ÄÚÀÏ")]copperCoil = 6122,
        [Description("È¸·Î±âÆÇ")]circuitBoard = 6123,
        [Description("¹ÝµµÃ¼")]semiconductor = 6124,
        [Description("ÄÜµ§¼­")]capacitor = 6125,
        [Description("Æ®·£Áö½ºÅÍ")]transistor = 6126,
        [Description("¹èÅÍ¸®")]battery = 6127,
        [Description("Àý¿¬Ã¼")]insulator = 6128,
        [Description("ÀÚ¼®")]magnet = 6129,
        
        [Description("º®µ¹")]brick = 6141,
        [Description("ÄÜÅ©¸®Æ®")]concrete = 6142,
        [Description("½Ã¸àÆ®")]cement = 6143,
        [Description("¼®Àç")]ston = 6144,
        [Description("Ã¶±Ù")]rebar = 6145,
        [Description("´Ü¿­Àç")]insulation = 6146,
        [Description("¹æ¼öÀç")]waterproofing = 6147,
        
        [Description("Ãµ")]cloth = 6161,
        [Description("°¡Á×")]leather = 6162,
        [Description("½Ç")]thread = 6163,
        [Description("¼¶À¯")]naturalFiber = 6164,
        
        [Description("À¯¸®")]glass = 6181,
        [Description("ÆÇÀÚ")]board = 6182,
        [Description("ÇÃ¶ó½ºÆ½")]profit = 6183,
        [Description("³ª»ç")]screw = 6184,
        
        //±â°è
        [Description("ºí¸®º£·ç")] bliveru = 7001,
        
        [Description("ºó")] notting = 7100,
        [Description("ÇÁ¶óÀÌÆÒ")] fryingPan = 7101,
        [Description("³¿ºñ")] pot = 7102,
        [Description("¿Àºì")] oven = 7103,
        [Description("¹ä¼Ü")] riceCooker = 7104,
        [Description("ÀüÀÚ·¹ÀÎÁö")] microwaveOven = 7105,
        
        [Description("Åé")] saw = 7110,
        [Description("µå¶óÀÌ¹ö")] screwdriver = 7111,
        [Description("¿ëÁ¢±â")] welder = 7112,
        [Description("¿ëÇØ·Î")] furnace = 7113,
        [Description("À¯¸® ºí·Î¿ì ÆÄÀÌÇÁ")] blowPipe = 7114,
        [Description("º¸¼® Àý´Ü±â")] jewelCutter = 7115,
        [Description("ÆÐ½Ë ¸Ó½Å")] facetMachine = 7116,
        [Description("¹ßÀü±â")] generator = 7117,
        [Description("³³¶«ÀÎµÎ")] solderingIron = 7118,
        [Description("¿ÍÀÌ¾î Ä¿ÅÍ")] wireCutter = 7119,
        [Description("¹ß¹°·¹")]  spinningWheel = 7120,
        
        [Description("º° ¸ð¾ç º¸¼®")]starJewelry = 8001,
    }
}