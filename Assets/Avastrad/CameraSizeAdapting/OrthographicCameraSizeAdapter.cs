using System;
using UnityEngine;

namespace Avastrad.CameraSizeAdapting
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class OrthographicCameraSizeAdapter : MonoBehaviour
    {
        [SerializeField] private Vector2Int referenceResolution = new(1920, 1080);
        [SerializeField] private Vector2 offsetPower = new(0, 1);

        private const float AspectTolerance = 0.0001f;
        private Camera _camera;
        private float _prevAspect;
        private float _refAspect;
        private float _defaultSize;
        private Vector3 _defaultPosition;
        
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _refAspect = (float)referenceResolution.y / referenceResolution.x;
            _defaultSize = _camera.orthographicSize;
            _defaultPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var curAspect = (float)Screen.height / Screen.width;

            if(Math.Abs(curAspect - _prevAspect) < AspectTolerance)
                return;
            
            if (_refAspect < curAspect)
            {
                var newSize = _defaultSize * curAspect / _refAspect;
                _camera.orthographicSize = newSize;

                var vertOffset = Vector3.up * (offsetPower.y * (newSize - _defaultSize));
                var horOffset = Vector3.right * (offsetPower.x * (newSize - _defaultSize));
                var newPosition = _defaultPosition + vertOffset + horOffset;
                transform.position = newPosition;
            }
            else
            {
                _camera.orthographicSize = _defaultSize;
                transform.position = _defaultPosition;
            }

            _prevAspect = curAspect;
        }
    }
}