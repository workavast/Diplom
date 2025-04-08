using UnityEngine;

namespace App.Ai.Config
{
    [CreateAssetMenu(fileName = nameof(AiConfig), menuName = Consts.AppName + "/Configs/" + nameof(AiConfig))]
    public class AiConfig : ScriptableObject
    {
        [field: Header("Vision")]
        [field: SerializeField] public LayerMask PlayerLayers { get; private set; }
        [field: SerializeField, Min(0)] public float ViewRadius { get; private set; } = 5f;
        
        [field: Header("Behavior")]
        [field: SerializeField, Min(0)] public float AttackDistance { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public float MoveTolerance { get; private set; } = 0.01f;

        [field: Header("Modules")]
        [field: SerializeField] public CombatConfig CombatConfig { get; private set; }
        [field: Space]
        [field: SerializeField] public ChaseConfig ChaseConfig { get; private set; }
        [field: Space]
        [field: SerializeField] public HoldPositionConfig HoldPositionConfig { get; private set; }
    }
}