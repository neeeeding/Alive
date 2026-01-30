using JetBrains.Annotations;
using UnityEngine;

namespace _02Script.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] protected Player[] characters;
        [CanBeNull] public static Player curPlayer;

        protected virtual void SelectPlayer([CanBeNull] Player curP)
        {
            foreach (Player p in characters)
            {
                p.isCurPlayer = false;
            }
            if (curP == null)
            {
                curPlayer = null;
                return; 
            }
            curPlayer = curP;
            curPlayer.isCurPlayer = true;
        }

        private void Awake()
        {
            SelectPlayer(null);
        }

        protected virtual void OnEnable()
        {
            Player.OnSelectPlayer += SelectPlayer;
        }

        protected virtual void OnDisable()
        {
            Player.OnSelectPlayer -= SelectPlayer;
        }
    }
}