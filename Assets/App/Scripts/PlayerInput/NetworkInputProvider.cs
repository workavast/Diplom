using System;
using System.Collections.Generic;
using App.PlayerInput.InputProviding;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Zenject;

namespace App.PlayerInput
{
    [DisallowMultipleComponent]
    public class NetworkInputProvider : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Inject] private readonly IInputProvider _inputProvider;

        private Vector2 _lastLookDirection;
        
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var movementInput = Vector2.zero;

            if (!_inputProvider.MouseOverUI())
            {
                _lastLookDirection = _inputProvider.GetLookDirection(runner.LocalPlayer);
                movementInput.x = _inputProvider.MoveDirection.x;
                movementInput.y = _inputProvider.MoveDirection.y;
            }
            
            var playerInputData = new PlayerInputData {
                HorizontalInput = movementInput.x,
                VerticalInput = movementInput.y,
                LookDirection = _lastLookDirection
            };
            
            playerInputData.Buttons.Set(PlayerButtons.Fire, _inputProvider.Fire && !_inputProvider.MouseOverUI());
            playerInputData.Buttons.Set(PlayerButtons.Reload, _inputProvider.Reload && !_inputProvider.MouseOverUI());
            playerInputData.Buttons.Set(PlayerButtons.Sprint, _inputProvider.Sprint);

            input.Set(playerInputData);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.LogError("OnPlayerJoined");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.LogError("Connected");
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }
        
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }
        
        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }
    }
}