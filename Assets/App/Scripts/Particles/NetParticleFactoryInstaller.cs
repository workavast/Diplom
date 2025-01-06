using Zenject;

namespace App.Particles
{
    public class NetParticleFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var netParticleFactory = FindFirstObjectByType<NetParticlesFactory>();
            Container.BindInterfacesAndSelfTo<NetParticlesFactory>().FromInstance(netParticleFactory);
        }
    }
}