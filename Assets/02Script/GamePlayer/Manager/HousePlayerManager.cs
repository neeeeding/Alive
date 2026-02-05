using _02Script.GamePlayer.GamePlayer;
using UnityEngine;

namespace _02Script.GamePlayer.Manager
{
    public class HousePlayerManager : PlayerManager
    {
        [SerializeField] private GameObject followCamera;
        protected override void SelectPlayer(Player curP)
        {
            base.SelectPlayer(curP);
            SetFollowCamera();
        }

        private void SetFollowCamera()
        {
            followCamera.transform.SetParent(curPlayer.transform);
            followCamera.transform.position = curPlayer.transform.position;
        }

        protected override void Awake()
        {
            SelectPlayer(characters[0]);
        }
    }
}