using System.Collections.Generic;
using _02Script.Battle.Entity;
using _02Script.GamePlayer.State;
using UnityEngine;

namespace _02Script.GamePlayer.Movement
{
    public class BattleMonsterMovement : PlayerMovement
    {
        [Header("Battle--")]
        [SerializeField] private Vector2 targetOffset= new Vector2(1,0);

        private GameObject center;

        protected override void OnEnable()
        {
            center = Camera.main.gameObject;
            BattleEntity.OnTarget += Target;
            BattleEntity.OnAction += ChangeAnimation;
        }

        protected override void OnDisable()
        {
            BattleEntity.OnTarget -= Target;
            BattleEntity.OnAction -= ChangeAnimation;
            base.OnDisable();
        }

        private void Target(List<BattleEntity> target, BattleEntity moveEntity)
        {
            if(moveEntity.gameObject != gameObject) return;
            Rd.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (target.Count <= 0)
            {
                TargetPos = center.transform.position;
                Rd.constraints = RigidbodyConstraints2D.FreezeAll;
                return;
            }
            
            if (target.Count == 1)
            {
                TargetPos = target[0].transform.position;
                TargetPos.y += targetOffset.y;
                TargetPos.x += TargetPos.x <transform.position.x? targetOffset.x : -targetOffset.x;
            }
            else
            {
                Vector3 sum = Vector3.zero;

                foreach (BattleEntity p in target)
                    sum += p.transform.position;

                TargetPos = sum / target.Count;
            }
            IsMoving = true;
            MoveStart();
        }

        private void ChangeAnimation(PlayerStateType animationType)
        {
            player.ChangeState(animationType,(int)player.Animator.GetFloat(X),(int)player.Animator.GetFloat(Y));
        }
    }
}