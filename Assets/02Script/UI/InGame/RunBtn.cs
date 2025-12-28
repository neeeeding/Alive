using System;
using _02Script.Player.State;
using UnityEngine;


namespace _02Script.UI.InGame
{
    public class RunBtn : MonoBehaviour
    {
        private float walk = 5;
        public static Action<float> OnMoveSpeed;

        public void Run()
        {
            OnMoveSpeed?.Invoke(walk * 2);
        }

        public void Walk()
        {
            OnMoveSpeed?.Invoke(walk);
        }
    }
}
