using App.ScenesLoading;
using Avastrad.ScenesLoading;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.NetworkRunning.Shutdowners.LocalShutdowners
{
    public class LobbyShutdowner:LocalShutdowner
    {
        private readonly ISceneLoader _sceneLoader;

        public LobbyShutdowner(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public override void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (SceneManager.GetActiveScene().buildIndex == ScenesConfig.MainMenuSceneIndex)
            {
                Debug.LogWarning("You are trying return in the menu when you already in the menu");
                return;
            }

            _sceneLoader.LoadScene(ScenesConfig.MainMenuSceneIndex, true, true);
        }
    }
}