using UnityEngine;

namespace App.Weapons
{
    [CreateAssetMenu(fileName = nameof(WeaponViewConfig), menuName = Consts.AppName + "/Configs/Weapon/" + nameof(WeaponViewConfig))]
    public class WeaponViewConfig : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem ShotSmoke { get; private set; }
        [field: SerializeField, Range(0,1)] public float MaxPitchOffset { get; private set; }
    }
}