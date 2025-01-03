using System;
using Avastrad.UI.UiSystem;
using UnityEngine;
using Zenject;

namespace BlackRed.Game.UI
{
    public class ScreensControllerInstaller : MonoInstaller
    {
        [SerializeField] private ScreensController screensController;
        
        public override void InstallBindings()
        {
            if (screensController == null)
            {
                Debug.LogWarning($"Ypo forgot serialize {nameof(ScreensController)}");

                screensController = FindObjectOfType<ScreensController>();

                if (screensController == null)
                    throw new NullReferenceException($"Cant find {nameof(ScreensController)} on the scene");
            }

            Container.BindInterfacesAndSelfTo<ScreensController>().FromInstance(screensController).AsSingle();
        }
    }
}