using UnityEngine;
using Zenject;

namespace App.Damage
{
    public class DamageApplicatorInstaller : MonoInstaller
    {
        [SerializeField] private bool hasFriendlyFire;
        
        public override void InstallBindings()
        {
            Debug.Log("InstallBindings - InstallBindings");
            Container.BindInterfacesAndSelfTo<DamageApplicator>().FromNew().AsSingle().WithArguments(hasFriendlyFire).NonLazy();
        }
    }
}