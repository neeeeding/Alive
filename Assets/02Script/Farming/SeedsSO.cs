using System;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.Farming
{
    [CreateAssetMenu(fileName = "SeedsSO", menuName = "SO/Farming/SeedsSO")]
    public class SeedsSO : ScriptableObject
    {
        public ItemDataSO seeds;
        public ItemDataSO fruit;
        public TemperatureType temperatureType;
        public float growDelay = 60 * 5;
    }

    /**온도*/
    [Flags]
    public enum TemperatureType
    {
        none = 0,
        frigid =1 <<0, //한랭
        cold = 1<<1, //서늘
        warmth = 1<<2, //온난
        highTemperature = 1<<3, //고온
        dry = 1<<4 //건조
    }
}