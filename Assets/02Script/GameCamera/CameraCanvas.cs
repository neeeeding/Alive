using UnityEngine;
using UnityEngine.UI;

namespace _02Script.GameCamera
{
    public class CameraCanvas : MonoBehaviour
    {
        [Header("Target Aspect Ratio")] [SerializeField]
        protected float targetAspect = 16f / 9f; // 1920 x 1080

        [Header("References")] [SerializeField]
        protected Camera targetCamera;

        [SerializeField] private Canvas targetCanvas;

        protected float currentAspect;
        protected float scaleHeight;
        protected float scaleWidth;

        protected virtual void Start()
        {
            if (targetCamera == null)
                targetCamera = Camera.main;

            if (targetCanvas == null)
                targetCanvas = FindObjectOfType<Canvas>();

            ApplyAspectRatio();
        }

        protected virtual void Update()
        {
            float newAspect = (float)Screen.width / Screen.height;
            if (Mathf.Abs(currentAspect - newAspect) > 0.01f)
            {
                ApplyAspectRatio();
            }
        }

        protected virtual void ApplyAspectRatio()
        {
            currentAspect = (float)Screen.width / Screen.height;
            AdjustCamera();
            AdjustCanvas();
        }

        protected virtual void AdjustCamera()
        {
            scaleHeight = currentAspect / targetAspect;
            scaleWidth = 1f / scaleHeight;

            Rect rect = targetCamera.rect;

            if (scaleHeight < 1f)
            {
                rect.width = 1f;
                rect.height = scaleHeight;
                rect.x = 0f;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0f;
            }

            targetCamera.rect = rect;
        }

        protected virtual void AdjustCanvas()
        {
            if (targetCanvas == null) return;

            CanvasScaler canvasScaler = targetCanvas.GetComponent<CanvasScaler>();
            if (canvasScaler == null)
            {
                canvasScaler = targetCanvas.gameObject.AddComponent<CanvasScaler>();
            }
            
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            if (scaleHeight < 1f)
                canvasScaler.matchWidthOrHeight = 0f;
            else
                canvasScaler.matchWidthOrHeight = 1f;

            RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                Vector2 sizeDelta = canvasRect.sizeDelta;

                if (scaleHeight < 1f)
                {
                    sizeDelta.y = 1080f;
                    sizeDelta.x = sizeDelta.y * currentAspect;
                }
                else
                {
                    sizeDelta.x = 1920f;
                    sizeDelta.y = sizeDelta.x / currentAspect;
                }
                canvasRect.sizeDelta = sizeDelta;
            }
        }

        protected virtual void OnPreCull() => GL.Clear(true, true, Color.black);
        
        protected virtual void OnValidate()
        {
            if (Application.isPlaying)
            {
                ApplyAspectRatio();
            }
        }
    }
}