using UnityEngine;

namespace _02Script.GameCamera
{
    public class BattleCameraCanvas : CameraCanvas
    {
        [SerializeField] private Camera cam2; //처음에 아래

        private bool _swapped; // true면 targetCamera가 아래, cam2가 위

        protected override void ApplyAspectRatio()
        {
            currentAspect = (float)Screen.width / Screen.height;
            AdjustSplitCamera(targetCamera, !_swapped);
            AdjustSplitCamera(cam2, _swapped);
        }

        public void SwapCameras()
        {
            _swapped = !_swapped;
            ApplyAspectRatio();
        }

        protected void AdjustSplitCamera(Camera cam, bool isTop)
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float halfHeightAspect = screenAspect * 2f;
            float scaleHeight = halfHeightAspect / targetAspect;
            float scaleWidth = 1f / scaleHeight;

            Rect rect = new Rect();

            if (scaleHeight < 1f)
            {
                float height = scaleHeight * 0.5f;
                rect.width = 1f;
                rect.height = height;
                rect.x = 0f;
                rect.y = isTop ? 0.5f : 0.5f - height;
            }
            else
            {
                rect.width = scaleWidth;
                rect.height = 0.5f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = isTop ? 0.5f : 0f;
            }

            cam.rect = rect;
        }
    }
}