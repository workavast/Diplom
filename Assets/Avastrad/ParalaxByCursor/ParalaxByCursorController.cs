using System;
using UnityEngine;

namespace Avastrad.ParalaxByCursor
{
    [DisallowMultipleComponent]
    public class ParalaxByCursorController : MonoBehaviour
    {
        [SerializeField] private Vector2 offsetPower;
        [field: SerializeField] public bool IsActive { get; private set; } = true;
        
        public event Action<Vector2> OnNewPosition;
        
        private void Awake()
        {
            var layerHolders = GetComponentsInChildren<LayerHolder>();
            foreach (var layerHolder in layerHolders) 
                layerHolder.Initialize(this);
        }

        private void Update()
        {
            if (!IsActive)
                return;

            var screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 mousePosition = Input.mousePosition;

            var fixedMouse = mousePosition - screenSize / 2;
            var offsetPercentageX = fixedMouse.x / screenSize.x / 2;
            var offsetPercentageY = fixedMouse.y / screenSize.y / 2;

            var offsetByX = Vector3.right * (offsetPercentageX * offsetPower.x);
            var offsetByY = Vector3.up * (offsetPercentageY  * offsetPower.y);
            var newPosition = offsetByX + offsetByY;
            newPosition.z = transform.position.z;
            
            OnNewPosition?.Invoke(newPosition);
        }

        public void ToggleActivity()
            => ToggleActivity(!IsActive);
        
        public void ToggleActivity(bool isActive) 
            => IsActive = isActive;
    }
}