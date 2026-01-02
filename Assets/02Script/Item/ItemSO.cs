using System.ComponentModel;
using UnityEngine;

namespace _02Script.Item
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public string itemName; //¾ÆÀÌÅÛ ÀÌ¸§
        public string itemExplanation;

        public ItemCategory category; //Ä«Å×°í¸®
        public ItemType itemType; //¾ÆÀÌÅÛ Á¾·ù

        public Sprite itemImage; //»ý±ä°Å
        public int maxCount;
    }

    public enum ItemCategory //Ä«Å×°í¸®
    {
        [Description("¾øÀ½")]none = 0,
        
        [Description("¾¾¾Ñ")]seed = 1,
        [Description("°î¹°")]fruit = 2,
        [Description("À½½Ä")]food = 3,
        
        [Description("¹«±â")]weapon = 4,
        [Description("°©¿Ê")]armor = 5,
        
        [Description("ºÎÇ°")]stuff = 6,
        [Description("±â°è")]machine = 7,
        [Description("±âÅ¸")]special = 8,
    }

    public enum ItemType //Á¾·ù
    {
        [Description("¾øÀ½")]none = 0, //¾ø´Ù.

        [Description("º­ ¾¾¾Ñ")]riceSeeds = 1001,
        [Description("º­")]rice = 2001,
        [Description("½Ò ¹ä")]warmRice = 3001,
        
        [Description("Ä®")]justKnife = 4001,
        [Description("¹æÆÐ")]justShield = 5001,
    }
}