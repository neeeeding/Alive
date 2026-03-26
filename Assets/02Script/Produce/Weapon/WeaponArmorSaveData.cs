using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;
using UnityEngine.Serialization;

namespace _02Script.Produce.Weapon
{    
    [Serializable]
    public class WeaponArmorSaveData
    {
        public ItemType type;
        public float hp;
        public string buffExplanation;
        public string explanation;
        
        public List<BuffType> buffTypes = new List<BuffType>();
    }
}