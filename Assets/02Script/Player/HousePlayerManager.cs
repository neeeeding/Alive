using UnityEngine;

namespace _02Script.Player
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

        private void Awake()
        {
            SelectPlayer(characters[0]);
        }
    }
}