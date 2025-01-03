using System;
using UnityEngine;

namespace App.Quitting
{
    public class QuitProvider : IQuitProvider, IQuitInvoker
    {
        public bool IsQuitting { get; private set; }
        
        public event Action OnQuit;
        
        public void Quit()
        {
            if (IsQuitting)
                return;

            IsQuitting = true;
            OnQuit?.Invoke();
            Application.Quit();
        }
    }
}