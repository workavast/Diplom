using UnityEngine;

namespace App.Weapons
{
    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = Consts.AppName + "/Configs/" + nameof(WeaponConfig))]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField] public WeaponId Id { get; private set; }
        [field: SerializeField, Min(1)] public int BulletsPerShoot { get; private set; } = 1;
        [field: SerializeField, Min(1)] public float DamagePerBullet { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float FireRate { get; private set; } = 0.2f;
        [field: SerializeField, Range(0, 90)] public float SpreadAngle { get; private set; } = 10f;
        [field: SerializeField, Min(1)] public int MagazineSize { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float ReloadTime { get; private set; } = 3;
    }
}