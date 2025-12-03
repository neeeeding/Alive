using _02Script.Player.State;
using UnityEngine;


namespace _02Script.UI.InGame
{
    public class RunBtn : MonoBehaviour
    {
        private float walk;

        private void Awake()
        {
            walk = PlayerMovement.Instance.speed;
        }

        public void Run()
        {
            PlayerMovement.Instance.speed = walk * 2;
        }

        public void Walk()
        {
            PlayerMovement.Instance.speed = walk;
        }
    }
}
