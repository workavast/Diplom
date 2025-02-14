using UnityEngine;

namespace App.CameraBehaviour
{
    [CreateAssetMenu(fileName = nameof(AimConfig), menuName = Consts.AppName + "/Configs/" + nameof(AimConfig))]
    public class AimConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float AimScale { get; private set; }  = 2;
    }
}