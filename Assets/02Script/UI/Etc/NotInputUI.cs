using _02Script.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02Script.UI.Etc
{
    public class NotInputUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            PlayerInput.Instance.NoInput();
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            PlayerInput.Instance.CanInput();
        }
    }
}