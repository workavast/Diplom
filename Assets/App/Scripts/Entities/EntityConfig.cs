using UnityEngine;

namespace App.Entities
{
    [CreateAssetMenu(fileName = nameof(EntityConfig), menuName = Consts.AppName + "/Configs/" + nameof(EntityConfig))]
    public class EntityConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public int InitialHealthPoints { get; private set; } = 100;
        [field: SerializeField, Min(0)] public int ReviveHealthPoints { get; private set; } = 10;
        [field: Header("Knockout")]
        [field: SerializeField, Min(0)] public int KnockoutHealthPoints { get; private set; } = 1;
        [field: SerializeField, Min(0)] public int KnockoutTime { get; private set; } = 15;
        [field: Header("Movement")]
        [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float SprintSpeed { get; private set; } = 7.5f;
        [field: SerializeField, Min(0)] public float MoveAcceleration { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float Gravity { get; private set; } = -9.8f;
    }
}