using System.Collections.Generic;
using _02Script.Inventory.Item;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.Battle.Entity
{
    [CreateAssetMenu(fileName = "BattleEntitySO", menuName = "SO/Entity/Battle", order = 0)]
    public class BattleEntitySO : EntitySO
    {
        [Space(25f)]
        [Header("BattleEntitySO------------------------")]
        public List<WeaponItemDataSO> useWeapons = new List<WeaponItemDataSO>(); //이넘의 경우 양이 너무 많아서...
    }
}