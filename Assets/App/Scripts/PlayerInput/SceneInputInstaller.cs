using System;
using App.PlayerInput.InputProviding;
using UnityEngine;
using Zenject;

namespace App.PlayerInput
{
    public class SceneInputInstaller : MonoInstaller
    {
        [SerializeField] private InputType inputType;
        
        public override void InstallBindings()
        {
            switch (inputType)
            {
                case InputType.Default:
                    Container.Bind<DefaultInputProvider>().FromNew().AsSingle().NonLazy();
                    break;
                case InputType.MainMenu:
                    Container.Bind<MainMenuInputProvider>().FromNew().AsSingle().NonLazy();
                    break;
                case InputType.Gameplay:
                    Container.Bind<GameplayInputProvider>().FromNew().AsSingle().NonLazy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum InputType
        {
            Default = 0,
            MainMenu = 10,
            Gameplay = 20
        }
    }
}