using UnityEngine;

namespace App.Entities.Reviving
{
    [CreateAssetMenu(fileName = nameof(ReviveConfig), menuName = Consts.AppName + "/Configs/" + nameof(ReviveConfig))]
    public class ReviveConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float ReviveDistance { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float ReviveTime { get; private set; } = 5f;
    }
}