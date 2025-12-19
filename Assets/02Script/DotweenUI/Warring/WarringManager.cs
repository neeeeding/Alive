using System;
using UnityEngine;

namespace JYE._01Script.UI.Warring
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