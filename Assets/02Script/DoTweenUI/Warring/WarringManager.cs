using UnityEngine;

namespace _02Script.DoTweenUI.Warring
{
    public class WarringManager :  MonoBehaviour
    {
        [SerializeField] private Warring warring;

        public static Warring Warring;

        private void Awake()
        {
            Warring = warring;
        }
    }
}