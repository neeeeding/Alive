using System;
using _02Script.Inventory.Item;
using UnityEngine;

namespace _02Script.GoHouse.SO
{
    [CreateAssetMenu(fileName = "BossItemSO", menuName = "SO/GoHouse/BossItemSO")]
    public class BossItemSO: BlockActionSO
    {
        public static Action<ItemDataSO, BlockActionSO> OnGetItem;
        
        public ItemDataSO bossGetItem; //보스의 부산물
        public override void DoBlockAction()
        {
            OnGetItem?.Invoke(bossGetItem,this);
        }
    }
}