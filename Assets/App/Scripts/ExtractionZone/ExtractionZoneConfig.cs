using UnityEngine;

namespace App.ExtractionZone
{
    [CreateAssetMenu(fileName = nameof(ExtractionZoneConfig), menuName = Consts.AppName + "/Configs/" + nameof(ExtractionZoneConfig))]
    public class ExtractionZoneConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float ExtractionRadius { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float ExtractionTime { get; private set; } = 5f;
    }
}