using UnityEngine;

namespace App.Ai
{
    [CreateAssetMenu(fileName = nameof(AiConfig), menuName = Consts.AppName + "/Configs/" + nameof(AiConfig))]
    public class AiConfig : ScriptableObject
    {
        [field: Header("Vision")]
        [field: SerializeField] public LayerMask PlayerLayers { get; private set; }
        [field: SerializeField, Min(0)] public float ViewRadius { get; private set; } = 5f;
        
        [field: Header("Combat")]
        [field: SerializeField] public float StayMinDuration { get; private set; } = 2f;
        [field: SerializeField] public float StayMaxDuration { get; private set; } = 4f;
        [field: SerializeField] public float WaitMinDuration { get; private set; } = 3f;
        [field: SerializeField] public float WaitMaxDuration { get; private set; } = 5f;
        [field: SerializeField] public float MoveMinDistance { get; private set; } = 3f;
        [field: SerializeField] public float MoveMaxDistance { get; private set; } = 3f;
        [field: SerializeField] public float MoveTolerance { get; private set; } = 0.01f;

        [field: Header("Behavior")]
        [field: SerializeField] public float AttackDistance { get; private set; } = 15f;
        [field: SerializeField] public float ChaseMinDuration { get; private set; } = 5f;
        [field: SerializeField] public float ChaseMaxDuration { get; private set; } = 5f;
        [field: SerializeField] public float HoldPositionMinDuration { get; private set; } = 5f;
        [field: SerializeField] public float HoldPositionMaxDuration { get; private set; } = 5f;

    }
}