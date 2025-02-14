using App.PlayerInput.InputProviding;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace App.CameraBehaviour
{
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private AimConfig config;
        
        [Inject] private readonly IInputProvider _inputProvider;

        private float _defaultValue;

        private void Awake()
        {
            _defaultValue = targetGroup.Targets[0].Weight;
        }

        private void LateUpdate()
        {
            if (_inputProvider.MouseOverUI())
                return;

            if (_inputProvider.Aim)
                targetGroup.Targets[0].Weight = _defaultValue * config.AimScale;
            else
                targetGroup.Targets[0].Weight = _defaultValue;
        }
    }
}