using System;
using App.NetworkRunning;
using App.ScenesLoading;
using App.UI;
using Avastrad.ScenesLoading;
using Fusion;
using Fusion.Photon.Realtime;
using shortid;
using shortid.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

namespace App.Session
{
    public class SessionCreator
    {
        private readonly NetworkRunnerProvider _networkRunnerProvider;
        private readonly WaitScreen _waitScreen;
        private readonly ISceneLoader _sceneLoader;

        private NetworkRunner NetworkRunner => _networkRunnerProvider.GetNetworkRunner();

        public SessionCreator(NetworkRunnerProvider networkRunnerProvider, WaitScreen waitScreen, ISceneLoader sceneLoader)
        {
            _networkRunnerProvider = networkRunnerProvider;
            _waitScreen = waitScreen;
            _sceneLoader = sceneLoader;
        }
        
        public void CreateSinglePlayer(int sceneIndex, Action successCallback = null, Action<ShutdownReason> failCallback = null)
            => StartGame(GameMode.Single, GenerateUid(), sceneIndex, successCallback, failCallback);
        
        public void QuickStart(Action successCallback = null, Action<ShutdownReason> failCallback = null) 
        {
            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                PlayerCount = 5,
                EnableClientSessionCreation = true,
                SessionNameGenerator = GenerateUid,
                IsOpen = true,
                IsVisible =  true,
                MatchmakingMode = MatchmakingMode.FillRoom,
                ObjectProvider = NetworkRunner.GetComponent<NetworkObjectPoolDefault>(),
            };

            StartGame(startGameArgs, ScenesConfig.GameplaySceneIndex, successCallback, failCallback);
        }
        
        public void ConnectToSession(string sessionName, int sceneIndex, Action successCallback = null, Action<ShutdownReason> failCallback = null) 
            => StartGame(GameMode.Client, sessionName, sceneIndex, successCallback, failCallback);
        
        public void CreateSession(string sessionName, int sceneIndex, Action successCallback = null, Action<ShutdownReason> failCallback = null)
            => StartGame(GameMode.Host, sessionName, sceneIndex, successCallback, failCallback);
        
        private void StartGame(GameMode gameMode, string sessionName, int sceneIndex, Action successCallback = null,
            Action<ShutdownReason> failCallback = null)
        {
            if (sessionName.IsNullOrEmpty()) 
                sessionName = GenerateUid();

            var startGameArgs = new StartGameArgs()
            {
                GameMode = gameMode,
                SessionName = sessionName,
                PlayerCount = 5,
                EnableClientSessionCreation = false,
                IsOpen = true,
                IsVisible =  true,
                SceneManager = NetworkRunner.GetComponent<NetworkSceneManagerDefault>(),
                ObjectProvider = NetworkRunner.GetComponent<NetworkObjectPoolDefault>()
            };

            StartGame(startGameArgs, sceneIndex, successCallback, failCallback);
        }
        
        private async void StartGame(StartGameArgs startGameArgs, int sceneIndex, Action successCallback = null,
            Action<ShutdownReason> failCallback = null)
        {
            // Let the Fusion Runner know that we will be providing user input
            NetworkRunner.ProvideInput = true;
            
            _waitScreen.Show();
            var result = await NetworkRunner.StartGame(startGameArgs);
            _waitScreen.Hide();
            
            if (result.Ok)
            {
                if (NetworkRunner.IsServer)
                {
                    if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
                    {
                        _sceneLoader.LoadScene(sceneIndex, true);
                        // var sceneName = ScenesData.NameByIndex(sceneIndex);
                        // await NetworkRunner.LoadScene(sceneName);
                    }
                }
                successCallback?.Invoke();
            }
            else
            {
                Debug.LogError($"{result.ErrorMessage} {result.ShutdownReason}");
                failCallback?.Invoke(result.ShutdownReason);
            }
        }

        private static string GenerateUid()
        {
            var options = new GenerationOptions(true, false, 8);
            return ShortId.Generate(options);
        }
    }
}