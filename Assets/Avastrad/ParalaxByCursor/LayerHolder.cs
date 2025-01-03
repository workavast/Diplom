using UnityEngine;

namespace Avastrad.ParalaxByCursor
{
    [DisallowMultipleComponent]
    public class LayerHolder : MonoBehaviour
    {
        [SerializeField] private Vector2 offsetPower = new Vector2(1, 0.5f);

        private ParalaxByCursorController _paralaxByCursorController;
        
        public void Initialize(ParalaxByCursorController paralaxByCursorController)
        {
            _paralaxByCursorController = paralaxByCursorController;
            _paralaxByCursorController.OnNewPosition += Move;
        }

        private void Move(Vector2 newPosition) 
            => transform.position = newPosition * offsetPower;

        private void OnDestroy() 
            => _paralaxByCursorController.OnNewPosition -= Move;
    }
}