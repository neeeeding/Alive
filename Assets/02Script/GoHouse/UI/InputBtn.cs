using System;
using UnityEngine;

namespace _02Script.GoHouse.UI
{
    public class InputBtn : MonoBehaviour
    {
        public static Action<Vector2> OnMoveBtn;

        [SerializeField] private Vector2 movePos;

        public void MoveBtn()
        {
            OnMoveBtn?.Invoke(movePos);
        }
    }
}