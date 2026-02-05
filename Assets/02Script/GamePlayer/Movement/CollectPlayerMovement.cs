using _02Script.Collect.Item;
using UnityEngine;

namespace _02Script.GamePlayer.Movement
{
    public class CollectPlayerMovement : PlayerMovement
    {
        [Header("Collect--")]
        [SerializeField] private Vector2 targetOffset= new Vector2(1,0.6f);
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
            if(!player.isCurPlayer) return;
            IsMoving = true;
            _wantItem = item;
            TargetPos = item.transform.position;
            TargetPos.y -= targetOffset.y;
            TargetPos.x += TargetPos.x <transform.position.x? targetOffset.x : -targetOffset.x;
            MoveStart();
        }

        protected override void Arrive()
        {
            base.Arrive();
            _wantItem.Gauge(player.playerName);
        }
    }
}