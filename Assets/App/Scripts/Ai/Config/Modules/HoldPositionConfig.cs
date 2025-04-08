using System;
using UnityEngine;

namespace App.Ai.Config
{
    [Serializable]
    public class HoldPositionConfig
    {
        [field: SerializeField, Min(0)] public float HoldPositionMinDuration { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float HoldPositionMaxDuration { get; private set; } = 5f;
        
        [field: Header("Movement")]
        [field: SerializeField, Min(0)] public float StayMinDuration { get; private set; } = 2f;
        [field: SerializeField, Min(0)] public float StayMaxDuration { get; private set; } = 4f;
        [field: Space]
        [field: SerializeField, Min(0)] public float MoveMinDistance { get; private set; } = 3f;
        [field: SerializeField, Min(0)] public float MoveMaxDistance { get; private set; } = 3f;
    }
}