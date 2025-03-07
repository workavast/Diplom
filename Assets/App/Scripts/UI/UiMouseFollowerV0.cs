using App.CameraBehaviour;
using App.PlayerInput.InputProviding;
using App.Players;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace App.UI
{
    public class UiMouseFollowerV0 : MonoBehaviour
    {
        [SerializeField] private Transform crosshair;
        [SerializeField] private NoiseConfig noiseConfig;
        
        [Inject] private readonly RawInputProvider _rawInputProvider;
        [Inject] private readonly LocalPlayerProvider _localPlayerProvider;

        private Sequence _shake;
        private Vector3 _defaultScale;
        
        private void Awake()
        {
            _defaultScale = transform.localScale;
            _localPlayerProvider.OnWeaponShot += ShotAnimation;
        }
        
        private void LateUpdate()
        {
            if (_rawInputProvider.MouseOverUI())
                return;

            crosshair.position = _rawInputProvider.MousePosition;
        }

        private void OnDestroy()
        {
            _localPlayerProvider.OnWeaponShot -= ShotAnimation;
        }

        private void ShotAnimation()
        {
            _shake?.Kill();

            var targetScale = _defaultScale * 1.5f;
            var duration = noiseConfig.TimeLenght / 2;

            crosshair.localScale = _defaultScale;
            var scaleUp = crosshair.DOScale(targetScale, duration).SetEase(Ease.OutQuad);
            var scaleDown = crosshair.DOScale(_defaultScale, duration).SetEase(Ease.InQuad);
            
            _shake = DOTween.Sequence()
                .Append(scaleUp)
                .Append(scaleDown)
                .SetLink(crosshair.gameObject)
                .OnKill(() => _shake = null);
        }
    }
}