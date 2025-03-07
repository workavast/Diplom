using App.PlayerInput.InputProviding;
using UnityEngine;
using Zenject;

namespace App.UI
{
    public class UiMouseFollower : MonoBehaviour
    {
        [SerializeField] private Transform crosshair;
        
        [Inject] private readonly RawInputProvider _rawInputProvider;

        private void LateUpdate()
        {
            if (_rawInputProvider.MouseOverUI())
                return;

            crosshair.position = _rawInputProvider.MousePosition;
        }
    }
}