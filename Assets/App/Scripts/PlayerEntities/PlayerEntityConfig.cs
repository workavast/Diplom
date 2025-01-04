using UnityEngine;

namespace App.PlayerEntities
{
    [CreateAssetMenu(fileName = nameof(PlayerEntityConfig), menuName = Consts.AppName + "/Configs/" + nameof(PlayerEntityConfig))]
    public class PlayerEntityConfig : ScriptableObject
    {
        [field: SerializeField] public int InitialHealthPoints { get; private set; } = 100;
        [field: SerializeField] public float AttackDaley { get; private set; } = 0.5f;
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5;
        [field: SerializeField] public float Gravity { get; private set; } = -9.8f;
        [field: SerializeField] public int Damage { get; private set; } = 10;
    }
}