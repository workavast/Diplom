using UnityEngine;
using Zenject;

namespace App.Armor
{
    public class ArmorInstaller : MonoInstaller
    {
        [SerializeField] private ArmorsConfig armorsConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<ArmorsConfig>().FromInstance(armorsConfig).AsSingle();
        }
    }
}