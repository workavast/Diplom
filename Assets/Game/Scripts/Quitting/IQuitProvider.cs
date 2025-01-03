using System;

namespace BlackRed.Game.Quitting
{
    public interface IQuitProvider
    {
        public bool IsQuitting { get; }
        
        public event Action OnQuit;
    }
}