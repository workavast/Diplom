using Unity.Cinemachine;
using UnityEngine;

namespace App.CameraBehaviour
{
    [CreateAssetMenu(fileName = nameof(NoiseConfig), menuName = Consts.AppName + "/Configs/" + nameof(NoiseConfig))]
    public class NoiseConfig : ScriptableObject
    {
        [field: SerializeField] public NoiseSettings NoiseSettings { get; private set; }
        [field: SerializeField, Min(0)] public float TimeLenght { get; private set; } = 1;
    }
}