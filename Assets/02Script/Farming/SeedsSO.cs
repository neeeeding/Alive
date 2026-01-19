using System;
using System.ComponentModel;
using _02Script.Inventory.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.Farming
{
    [CreateAssetMenu(fileName = "SeedsSO", menuName = "SO/Farming/SeedsSO")]
    public class SeedsSO : ScriptableObject
    {
        public ItemDataSO seeds;
        public ItemDataSO viand;
        public TemperatureType temperatureType;
        public float growDelay = 60 * 5;
    }

    /**온도*/
    [Flags]
    public enum TemperatureType
    {
        [Description("없음")]none = 0,
        [Description("한랭")]frigid =1 <<0, //한랭
        [Description("서늘")]cold = 1<<1, //서늘
        [Description("온난")]warmth = 1<<2, //온난
        [Description("고온")]highTemperature = 1<<3, //고온
        [Description("건조")]dry = 1<<4 //건조
    }
}