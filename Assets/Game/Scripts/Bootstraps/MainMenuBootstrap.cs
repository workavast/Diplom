using System;
using BugStrategy.ScenesLoading;
using UnityEngine;
using Zenject;

namespace BlackRed.Game.Bootstraps
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [Inject] private ISceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader.Initialize(false);
        }
    }
}