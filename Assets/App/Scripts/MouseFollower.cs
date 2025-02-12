using App.PlayerInput.InputProviding;
using Avastrad.Vector2Extension;
using UnityEngine;
using Zenject;

namespace App
{
    public class MouseFollower : MonoBehaviour
    {
        [Inject] private readonly RawInputProvider _rawInputProvider;
        
        private void Update()
        {
            if (_rawInputProvider.MouseOverUI())
                return;

            var depthOffset = Camera.main.transform.position.y;
            var screenPoint = _rawInputProvider.MousePosition.XY0(depthOffset);
            var lookPoint = Camera.main.ScreenToWorldPoint(screenPoint);

            transform.position = lookPoint;
        }
    }
}