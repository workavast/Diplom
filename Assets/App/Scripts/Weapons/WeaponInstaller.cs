using UnityEngine;
using Zenject;

namespace App.Weapons
{
    public class WeaponInstaller : MonoInstaller
    {
        [SerializeField] private WeaponsConfigs weaponsConfigs;
        
        public override void InstallBindings()
        {
            weaponsConfigs.Initialize(true);
            Container.BindInstance(weaponsConfigs).AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponFactory>().FromNew().AsSingle();
        }
    }
}