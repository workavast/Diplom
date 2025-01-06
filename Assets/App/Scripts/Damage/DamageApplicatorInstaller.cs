using UnityEngine;
using Zenject;

namespace App.Damage
{
    public class DamageApplicatorInstaller : MonoInstaller
    {
        [SerializeField] private bool hasPlayersFriendlyFire;
        [SerializeField] private bool hasEnemiesFriendlyFire;
        
        public override void InstallBindings()
        {
            Container.Bind<DamageApplicatorFactory>().FromNew().AsSingle().WithArguments(hasPlayersFriendlyFire, hasEnemiesFriendlyFire);
        }
    }
}