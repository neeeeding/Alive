using UnityEngine;
using UnityEngine.Serialization;

namespace _02Script.GameCamera
{
    public class CameraLimit : WheelMoveCameraInput
    {
        [SerializeField] private BoxCollider2D cameraLimit;

        private float _camWidth;
        private float _camHeight;
        private Bounds _mapSize;

        protected override void OnEnable()
        {
            base.OnEnable();
            _mapSize = cameraLimit.bounds;
            _camHeight = myCamera.orthographicSize;
            _camWidth = myCamera.aspect * _camHeight;

            maxZoomSize = Mathf.Min(maxZoomSize, Mathf.Min(_mapSize.size.x /5, _mapSize.size.y/3));
        }

        protected override void WheelSize()
        {
            base.WheelSize();
            _camHeight = myCamera.orthographicSize;
            _camWidth = myCamera.aspect * _camHeight;
        }

        protected override void WheelMove()
        {
            base.WheelMove();

            float minX = _mapSize.min.x + _camWidth;
            float maxX = _mapSize.max.x - _camWidth;

            float minY = _mapSize.min.y + _camHeight;
            float maxY = _mapSize.max.y - _camHeight;

            Vector3 pos = myCamera.transform.position;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            myCamera.transform.position = pos;
        }
    }
}