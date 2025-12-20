using System;
using UnityEngine;

namespace _02Script.DotweenUI.Warring
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