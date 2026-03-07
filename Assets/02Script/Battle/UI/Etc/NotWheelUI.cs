using _02Script.GameCamera;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02Script.Battle.UI.Etc
{
    public class NotWheelUI: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private WheelMoveCameraInput wheelInputScript;
        
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            wheelInputScript.WheelStop(false);
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            wheelInputScript.WheelStop(true);
        }
    }
}