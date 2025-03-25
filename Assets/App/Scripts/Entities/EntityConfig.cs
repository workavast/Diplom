using UnityEngine;

namespace App.Entities
{
    [CreateAssetMenu(fileName = nameof(EntityConfig), menuName = Consts.AppName + "/Configs/" + nameof(EntityConfig))]
    public class EntityConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int InitialHealthPoints { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float AttackDaley { get; private set; } = 0.5f;
        [field: Header("Movement")]
        [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float SprintSpeed { get; private set; } = 7.5f;
        [field: SerializeField, Min(0)] public float MoveAcceleration { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float Gravity { get; private set; } = -9.8f;
    }
}