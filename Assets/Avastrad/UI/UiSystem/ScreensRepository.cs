using System;
using System.Collections.Generic;
using App.UI.Screens;
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
                case ScreenType.Gameplay:
                    return GetScreen<GameplayScreen>();
                case ScreenType.GameplayMenu:
                    return GetScreen<GameplayMenuScreen>();
                case ScreenType.MainMenu:
                    return GetScreen<MainMenuScreen>();
                case ScreenType.Lobby:
                    return GetScreen<LobbyScreen>();
                case ScreenType.Settings:
                    return GetScreen<SettingsScreen>();
                default:
                    throw new ArgumentOutOfRangeException($"invalid parameter: {screenType}");
            }
        }
    }
}