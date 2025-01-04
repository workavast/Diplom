using Zenject;

namespace App.DiProviding
{
    public interface IDiProvider
    {
        public DiContainer DiContainer { get; }
    }
}