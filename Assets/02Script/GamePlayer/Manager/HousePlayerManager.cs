using _02Script.GamePlayer.GamePlayer;
using _02Script.Manager;
using _02Script.Obj.Entity;
using UnityEngine;

namespace _02Script.GamePlayer.Manager
{
    public class HousePlayerManager : PlayerManager
    {
        [SerializeField] private GameObject followCamera;
        protected override void SelectPlayer(Player curP)
        {
            base.SelectPlayer(curP);
            if(HouseManager.Instance.isStart)
                HouseManager.Instance.PlayerStat.playCharacter = curP.playerName;
            SetFollowCamera();
        }

        private void SetFollowCamera()
        {
            followCamera.transform.SetParent(curPlayer.transform);
            followCamera.transform.position = curPlayer.transform.position;
        }

        protected void StartPlayerSet()
        {
            Player p = null;
            foreach (Player c in characters)
            {
                if(HouseManager.Instance.PlayerStat.playCharacter ==  c.playerName)
                {
                    p = c;
                    break;
                }
            }
            SelectPlayer(p??characters[0]);
        }

        protected override void Awake()
        {
            SelectPlayer(characters[0]);
        }

        protected virtual void OnEnable()
        {
            base.OnEnable();
            HouseManager.OnStart += StartPlayerSet;
        }

        protected virtual void OnDisable()
        {
            base.OnDisable();
            HouseManager.OnStart -= StartPlayerSet;
        }
    }
}