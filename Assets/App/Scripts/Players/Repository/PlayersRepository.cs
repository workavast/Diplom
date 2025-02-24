using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace App.Players.Repository
{
    public class PlayersRepository : IReadOnlyPlayersRepository
    {
        public IReadOnlyList<PlayerRef> Players => _players;

        private readonly List<PlayerRef> _players = new(4);
        
        public event Action<PlayerRef> OnPlayerJoined;
        public event Action<PlayerRef> OnPlayerLeft;
        public event Action OnPlayerChanged;
        
        public void PlayerJoined(PlayerRef playerRef)
        {
            if (_players.Contains(playerRef))
            {
                Debug.LogWarning($"Player {playerRef} already joined");
                return;
            }

            _players.Add(playerRef);
            
            Debug.Log($"Player {playerRef} joined");
            OnPlayerJoined?.Invoke(playerRef);
            OnPlayerChanged?.Invoke();
        }

        public void PlayerLeft(PlayerRef playerRef)
        {
            if (!_players.Remove(playerRef)) 
                return;
            
            Debug.Log($"Player {playerRef} left");
            OnPlayerLeft?.Invoke(playerRef);
            OnPlayerChanged?.Invoke();
        }
    }

    public interface IReadOnlyPlayersRepository
    {
        public IReadOnlyList<PlayerRef> Players { get; }

        public event Action<PlayerRef> OnPlayerJoined;
        public event Action<PlayerRef> OnPlayerLeft;
        public event Action OnPlayerChanged;
    }
}