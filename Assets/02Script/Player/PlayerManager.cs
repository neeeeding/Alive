using System;
using UnityEngine;

namespace _02Script.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Player[] characters;
        private static Player curPlayer;

        private void SelectPlayer(Player curP)
        {
            foreach (Player p in characters)
            {
                p.isCurPlayer = false;
            }
            curPlayer = curP;
            curPlayer.isCurPlayer = true;
        }

        private void Awake()
        {
            SelectPlayer(characters[0]);
        }

        private void OnEnable()
        {
            Player.OnSelectPlayer += SelectPlayer;
        }

        private void OnDisable()
        {
            Player.OnSelectPlayer -= SelectPlayer;
        }
    }
}