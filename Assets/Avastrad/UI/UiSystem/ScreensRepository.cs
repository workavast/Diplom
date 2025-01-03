using System;
using System.Collections.Generic;
using Avastrad.UI.UiSystem.Example;
using UnityEngine;

namespace Avastrad.UI.UiSystem
{
    [DisallowMultipleComponent]
    public class ScreensRepository : MonoBehaviour
    {
        private readonly Dictionary<Type, ScreenBase> _screens = new();
    
        public IEnumerable<ScreenBase> Screens => _screens.Values;

        private void Awake()
        {
            var screens = GetComponentsInChildren<ScreenBase>(true);
            foreach (var screen in screens) 
                _screens.Add(screen.GetType(), screen);
        }

        public TScreen GetScreen<TScreen>() 
            where TScreen : ScreenBase
        {
            if(_screens.TryGetValue(typeof(TScreen), out var screen)) 
                return (TScreen)screen;

            return default;
        }
        
        public ScreenBase GetScreen(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.FirstScreen:
                    return GetScreen<FirstScreen>();
                case ScreenType.SecondScreen:
                    return GetScreen<SecondScreen>();
                case ScreenType.ThirdScreen:
                    return GetScreen<ThirdScreen>();
                default:
                    throw new ArgumentOutOfRangeException($"invalid parameter: {screenType}");
            }
        }
    }
}