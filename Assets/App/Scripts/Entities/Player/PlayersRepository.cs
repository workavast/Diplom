using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace App.Entities.Player
{
    public class PlayersRepository
    {
        private readonly Dictionary<PlayerRef, NetPlayerController> _players = new();
        private readonly Dictionary<int, NetPlayerController> _playersById = new();

        public event Action<PlayerRef, NetPlayerController> OnPlayerAdd;
        public event Action<PlayerRef> OnPlayerRemove;
        
        public void Add(PlayerRef playerRef, NetPlayerController player)
        {
            if (_players.ContainsKey(playerRef))
            {
                Debug.LogError($"Duplicate exception: {playerRef} | {player}");
                return;
            }
            else
            {
                _players.Add(playerRef, player);
                _playersById.Add(player.Identifier.Id, player);
            }
            
            OnPlayerAdd?.Invoke(playerRef, player);
        }
        
        public void Remove(NetPlayerController player)
        {
            _players.Remove(player.PlayerRef);
            _playersById.Remove(player.Identifier.Id);
            OnPlayerRemove?.Invoke(player.PlayerRef);
        }

        public bool TryGet(int identifier, out NetPlayerController player)
        {
            if (_playersById.TryGetValue(identifier, out var value))
            {
                player = value;
                return true;
            }

            player = null;
            return false;
        }
        
        public bool TryGet(PlayerRef playerRef, out NetPlayerController player)
        {
            if (_players.TryGetValue(playerRef, out var value))
            {
                player = value;
                return true;
            }

            player = null;
            return false;
        }
    }
}