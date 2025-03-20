using Zenject;

namespace App.Audio.Sources
{
    public class AudioSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactory();
        }

        private void BindFactory() 
            => Container.Bind<AudioFactory>().FromNew().AsSingle().NonLazy();
    }
}