using UnityEngine;

namespace App.Audio.Ambience
{
    [CreateAssetMenu(fileName = nameof(AmbienceConfig), menuName =  Consts.AppName + "/Configs/Audio/" + nameof(AmbienceConfig))]
    public class AmbienceConfig : ScriptableObject
    {
        [field: SerializeField] public float TransitionTime { get; private set; } = 1f;
    }
}