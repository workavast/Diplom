using UnityEngine;

namespace App.Ai
{
    [CreateAssetMenu(fileName = nameof(AiConfig), menuName = Consts.AppName + "/Configs/" + nameof(AiConfig))]
    public class AiConfig : ScriptableObject
    {
        [field: Header("Vision")]
        [field: SerializeField] public LayerMask PlayerLayers { get; private set; }
        [field: SerializeField, Min(0)] public float ViewRadius { get; private set; } = 5f;
        
        [field: Header("Combat-Movement")]
        [field: SerializeField, Min(0)] public float StayMinDuration { get; private set; } = 2f;
        [field: SerializeField, Min(0)] public float StayMaxDuration { get; private set; } = 4f;
        [field: SerializeField, Min(0)] public float WaitMinDuration { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float WaitMaxDuration { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float MoveMinDistance { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float MoveMaxDistance { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float MoveTolerance { get; private set; } = 0.01f;
        [field: Header("Combat-Weapon")]
        [field: SerializeField, Min(0)] public float PauseMinDuration { get; private set; } = 1f;
        [field: SerializeField, Min(0)] public float PauseMaxDuration { get; private set; } = 2f;
        [field: SerializeField, Min(0)] public int ShotsMinCount { get; private set; } = 2;
        [field: SerializeField, Min(0)] public int ShotsMaxCount { get; private set; } = 5;
        
        [field: Header("Behavior")]
        [field: SerializeField, Min(0)] public float AttackDistance { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public float ChaseMinDuration { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float ChaseMaxDuration { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float HoldPositionMinDuration { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float HoldPositionMaxDuration { get; private set; } = 5f;
    }
}