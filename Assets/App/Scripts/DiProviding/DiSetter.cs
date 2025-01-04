using Zenject;

namespace App.DiProviding
{
    public class DiSetter
    {
        public DiSetter(DiProvider diProvider, DiContainer diContainer)
        {
            diProvider.SetDiContainer(diContainer);
        }
    }
}