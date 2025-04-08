using System;
using UnityEngine;

namespace App.Ai.Config
{
    [Serializable]
    public class CombatConfig
    {
        [field: Header("Movement")]
        [field: SerializeField, Min(0)] public float StayMinDuration { get; private set; } = 2f;
        [field: SerializeField, Min(0)] public float StayMaxDuration { get; private set; } = 4f;
        [field: Space]
        [field: SerializeField, Min(0)] public float WaitMinDuration { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float WaitMaxDuration { get; private set; } = 5f;
        [field: Space]
        [field: SerializeField, Min(0)] public float MoveMinDistance { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float MoveMaxDistance { get; private set; } = 3f;
        [field: Header("Weapon")]
        [field: SerializeField, Min(0)] public float PauseMinDuration { get; private set; } = 1f;
        [field: SerializeField, Min(0)] public float PauseMaxDuration { get; private set; } = 2f;
        [field: Space]
        [field: SerializeField, Min(0)] public int ShotsMinCount { get; private set; } = 2;
        [field: SerializeField, Min(0)] public int ShotsMaxCount { get; private set; } = 5;
    }
}