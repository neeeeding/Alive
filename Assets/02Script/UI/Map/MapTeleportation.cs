using _02Script.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace _02Script.UI.Map
{
    public class MapTeleportation : MonoBehaviour
    {
        [Header("Need")]
        [SerializeField] private RawImage mapImage; // 맵
        [SerializeField] private Camera mainCamera; //카메라
        [SerializeField] private Transform inGameMap;
        [SerializeField] private Canvas canvas;

        private RectTransform imageRect; //맵 크기
        
        private void Awake()
        {
            imageRect = mapImage.GetComponent<RectTransform>();
        }
        
        public void TeleportationPos()
        {
            Vector2 mapSize = imageRect.rect.size/2; //맵 사이즈
            Vector2 mapPosition = imageRect.position; //맵 위치
            
            Camera uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imageRect, Input.mousePosition, uiCam, out Vector2 mapPos);
            
            if(!ConvertRawImageToWorldPos(mapPos + mapSize, out Vector3 playerPos)) return; //범위 외. 실패
            
            GameManager.Instance.Player.transform.position = playerPos;
        }
        
        private bool ConvertRawImageToWorldPos(Vector3 mapPos, out Vector3 playerPos)
        {
            Vector3 worldPos = mainCamera.ViewportToScreenPoint(mapPos);
            worldPos.z = -1;
            
            bool success = RectTransformUtility.ScreenPointToWorldPointInRectangle(imageRect, mapPos, mainCamera, out Vector3 worldPoint);

            worldPoint.z = 0;
            playerPos = worldPoint;
            return  success;
        }
    }
}