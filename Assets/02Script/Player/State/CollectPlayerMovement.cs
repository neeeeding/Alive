using System;
using _02Script.Obj.Item;
using UnityEngine;

namespace _02Script.Player.State
{
    public class CollectPlayerMovement : PlayerMovement
    {
        private CollectItem _wantItem;

        protected override void OnEnable()
        {
            base.OnEnable();
            CollectItem.OnClickItem += Move;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CollectItem.OnClickItem -= Move;
        }

        private void Move(CollectItem item)
        {
            TargetPos = item.transform.position;
            _wantItem = item;
        }

        protected override void Arrive()
        {
            base.Arrive();
            _wantItem.Gauge();
        }
    }
}