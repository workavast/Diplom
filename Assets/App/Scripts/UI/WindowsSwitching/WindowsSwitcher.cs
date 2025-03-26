using System.Collections.Generic;
using Avastrad.CheckOnNullLibrary;
using UnityEngine;

namespace App.UI.WindowsSwitching
{
    public class WindowsSwitcher : MonoBehaviour
    {
        private readonly Dictionary<string, IWindow> _windows = new(0);

        private IWindow _activeWindow;
        
        private void Awake()
        {
            var windows = GetComponentsInChildren<IWindow>();
            
            _windows.EnsureCapacity(windows.Length);
            foreach (var window in windows)
            {
                _windows.Add(window.Id, window);
                window.Toggle(false);
            }
        }

        public void SwitchWindow(string id)
        {
            HideActiveWindow();

            _activeWindow = _windows[id];

            _activeWindow.Toggle(true);
        }

        public void HideActiveWindow()
        {
            if (!_activeWindow.IsAnyNull()) 
                _activeWindow.Toggle(false);

            _activeWindow = null;
        }
    }
}