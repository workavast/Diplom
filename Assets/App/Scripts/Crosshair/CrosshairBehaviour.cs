using App.Players;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace App.Crosshair
{
    public class CrosshairBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform crosshair;
        [SerializeField] private CrosshairConfig config;
        
        [Inject] private readonly LocalPlayerProvider _localPlayerProvider;

        private Sequence _shake;
        private Vector3 _defaultScale;
        
        private void Awake()
        {
            _defaultScale = transform.localScale;
            _localPlayerProvider.OnWeaponShot += ShotAnimation;
        }
        
        private void OnDestroy()
        {
            _localPlayerProvider.OnWeaponShot -= ShotAnimation;
        }

        private void ShotAnimation()
        {
            _shake?.Kill();

            var targetScale = _defaultScale * config.TargetScale;

            crosshair.localScale = _defaultScale;
            var scaleUp = crosshair.DOScale(targetScale, config.ScaleUpDuration).SetEase(config.ScaleUpEase);
            var scaleDown = crosshair.DOScale(_defaultScale, config.ScaleDownDuration).SetEase(config.ScaleDownEase);
            
            _shake = DOTween.Sequence()
                .Append(scaleUp)
                .Append(scaleDown)
                .SetLink(crosshair.gameObject)
                .OnKill(() => _shake = null);
        }
    }
}