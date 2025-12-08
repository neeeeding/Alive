using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _02Script.Item
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public string itemName; //아이템 이름

        public ItemCategory category; //카테고리
        public ItemType itemType; //아이템 종류

        public int sellCoin; //파는 가격

        public Sprite itemImage; //생긴거
    }

    public enum ItemCategory //카테고리
    {
        food = 1000,
        stuff = 2000, //기계 부품
        machine = 3000,
        special = 4000,
        
        seed = 5000,
        fruit = 6000,
        
        weapon = 7000,
        armor = 8000,
        
        none = 0
    }

    public enum ItemType //종류
    {
        none = 0, //없다.

        warmRice = 1001,
        
        rice = 5001,
        
        riceSeeds = 6001,
        
        justKnife = 7001,
        justShield = 8001,
    }
}