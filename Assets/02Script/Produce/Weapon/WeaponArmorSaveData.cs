using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using _02Script.Inventory.Item;

namespace _02Script.Produce.Weapon
{    
    [Serializable]
    public class WeaponArmorSaveData
    {
        public ItemType type;
        public float hp;
        
        public List<BuffType> buffTypes = new List<BuffType>();
    }
}