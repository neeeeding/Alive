using System;
using _02Script.Manager;
using UnityEngine;


namespace _02Script.UI.InGame
{
    public class RunBtn : MonoBehaviour
    {
        public static Action<float> OnMoveSpeed;

        public void Run()
        {
            OnMoveSpeed?.Invoke(GameManager.Instance.RunSpeed);
        }

        public void Walk()
        {
            OnMoveSpeed?.Invoke(GameManager.Instance.WalkSpeed);
        }
    }
}
