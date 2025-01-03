using System;

namespace App.Quitting
{
    public interface IQuitProvider
    {
        public bool IsQuitting { get; }
        
        public event Action OnQuit;
    }
}