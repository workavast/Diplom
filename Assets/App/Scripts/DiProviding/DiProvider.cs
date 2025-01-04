using Zenject;

namespace App.DiProviding
{
    public class DiProvider : IDiProvider
    {
        public DiContainer DiContainer { get; private set; }

        public void SetDiContainer(DiContainer diContainer)
        {
            DiContainer = diContainer;
        }
    }
}