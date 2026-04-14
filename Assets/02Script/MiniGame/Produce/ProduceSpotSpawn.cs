using UnityEngine;

namespace _02Script.MiniGame.Produce
{
    public class ProduceSpotSpawn : MiniGameObjSpawn
    {
        [SerializeField] private ProduceScore score;

        private void OnEnable()
        {
            minTime = 0.15f;
            maxTime = 1f;
        }

        protected override void ObjSetting()
        {
            base.ObjSetting();
            ProduceSpot spot = _spotList[0] as ProduceSpot;
            spot.SetSpot(score);
        }

        public override void ObjListAdd(MiniGameObj obj)
        {
            base.ObjListAdd(obj);
            SetTime();
        }
    }
}