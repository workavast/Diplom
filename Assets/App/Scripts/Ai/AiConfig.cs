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
        [field: SerializeField] public float PositionChangeInterval { get; private set; } = 5f;
        [field: SerializeField] public float PositionChangeDistance { get; private set; } = 3f;

        [field: Header("Behavior")]
        [field: SerializeField] public float ChaseDuration { get; private set; } = 10f;
        [field: SerializeField] public float WaitDuration { get; private set; } = 5f;
        [field: SerializeField] public float AttackDistance { get; private set; } = 15f;
    }
}