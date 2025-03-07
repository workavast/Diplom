using DG.Tweening;
using UnityEngine;

namespace App.Crosshair
{
    [CreateAssetMenu(fileName = nameof(CrosshairConfig), menuName = Consts.AppName + "/Configs/" + nameof(CrosshairConfig))]
    public class CrosshairConfig : ScriptableObject
    {
        [field: SerializeField] public float TargetScale { get; private set; }
        [field: SerializeField] public Ease ScaleUpEase { get; private set; } = Ease.OutQuad;
        [field: SerializeField, Min(0)] public float ScaleUpDuration { get; private set; }
        [field: SerializeField] public Ease ScaleDownEase { get; private set; } = Ease.InQuad;
        [field: SerializeField, Min(0)] public float ScaleDownDuration { get; private set; }
    }
}