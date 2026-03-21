using System;
using System.Collections.Generic;
using _02Script.Battle.Buff;
using UnityEngine;

namespace _02Script.Battle.UI.Weapon
{
    public class BuffFind : MonoBehaviour
    {
        [SerializeField] private BuffSO[] allBuffSO;
        private Dictionary<BuffType, BuffSO> buffs = new Dictionary<BuffType, BuffSO>();
        
        private void Awake()
        {
            BuffSetting();
        }

        public BuffSO GetBuff(BuffType buffType)
        {
            if (!buffs.ContainsKey(buffType)) return null;
            return buffs[buffType];
        }

        private void BuffSetting()
        {
            buffs.Clear();
            foreach (BuffSO buff in allBuffSO)
            {
                buffs.Add(buff.buffType, buff);
            }
        }
    }
}