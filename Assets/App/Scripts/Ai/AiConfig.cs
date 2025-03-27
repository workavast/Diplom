using UnityEngine;

namespace App.Ai
{
    [CreateAssetMenu(fileName = nameof(AiConfig), menuName = Consts.AppName + "/Configs/" + nameof(AiConfig))]
    public class AiConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask PlayerLayers { get; private set; }
        [field: SerializeField, Min(0)] public float ViewRadius { get; private set; } = 5f;
    }
}