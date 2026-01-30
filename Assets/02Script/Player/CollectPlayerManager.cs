using UnityEngine;

namespace _02Script.Player
{
    public class CollectPlayerManager : PlayerManager
    {
        [SerializeField] private GameObject selectPlayerMark;
        protected override void SelectPlayer(Player curP)
        {
            base.SelectPlayer(curP);
            SetFollowCamera();
        }
        private void SetFollowCamera()
        {
            if (curPlayer == null)
            {
                selectPlayerMark.SetActive(false);
                return;
            }
            selectPlayerMark.SetActive(true);
            selectPlayerMark.transform.SetParent(curPlayer.transform);
            selectPlayerMark.transform.position = curPlayer.transform.position;
        }
    }
}