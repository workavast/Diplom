using System;
using UnityEngine;

namespace App.Ai.Config
{
    [Serializable]
    public class ChaseConfig
    {
        [field: SerializeField, Min(0)] public float ChaseMinDuration { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float ChaseMaxDuration { get; private set; } = 5f;
    }
}